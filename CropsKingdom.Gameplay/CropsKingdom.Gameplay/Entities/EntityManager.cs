using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace CropsKingdom.Gameplay.Entities
{
    public class EntityManager
    {
        private Entity[] _entities;
        private int _currentId;
        private int[] _availableIds;
        private int _currentAvailableIdsPointer;
        private int _entitiesCount;

        public EntityManager(int startingSize = 128)
        {
            _entities = new Entity[startingSize];
            _currentId = 0;
            _entitiesCount = 0;
            _availableIds = new int[startingSize];
            _currentAvailableIdsPointer = 0;

            // Reserved entity at index 0.
            // This is to reserve index 0 from external usage, since it is used to mark entities as unset.
            // I didn't use -1 or other values because:
            // - _entities array entries are automatically initialized to default(Entity) also when empty
            // - I want to avoid branching when iterating on all entities (to filter unset entities), to avoid branch mis-prediction
            // Instead I will assign a default (probably empty) behavior on entity 0, so the compiler should be able to
            // vectorize the loop.
            // An additional entity will not slow the code, but in this case it can improve performance instead.
            // All unset entities will point to this empty component.
            // Btw, unset entities will last for few frames, until re-compacting.
            _entities[0] = default(Entity);
        }

        public int GetEntitiesCount()
        {
            return _entitiesCount;
        }

        public bool IsCompacted()
        {
            return _currentAvailableIdsPointer == 0;
        }
        
        public Entity CreateEntity()
        {
            int id = -1;

            if (_currentAvailableIdsPointer > 0)
            {
                var index = Interlocked.Decrement(ref _currentAvailableIdsPointer);
                id = _availableIds[index];
            }
            else
            {
                id = Interlocked.Increment(ref _currentId);
            }
            
            EnsureEntitiesCapacity(id);
            _entities[id].Id = id;

            Interlocked.Increment(ref _entitiesCount);
            
            return _entities[id];
        }

        public void DestroyEntity(Entity entity)
        {
            // Id 0 is reserved, you can't destroy it. This check will disappear in Release mode.
            Debug.Assert(entity.Id != 0);
            
            // I will not decrement the entities counter. I will do it in RecompactEntities function, usually  
            // executed as last operation in the current frame.
            // I just mark the entity as unset and push its ID in the availableIDs pool.
            UnsetEntity(entity.Id);

            // TODO: unset components too.
            
            EnsureIdentifiersCapacity();
            _availableIds[Interlocked.Increment(ref _currentAvailableIdsPointer)] = entity.Id;
        }

        public void RecompactEntities()
        {
            // Locking at this time should be ok: no contention should happen, since this is the last function called 
            // in the frame, after every other updating logic.
            lock (_entities)
            {
                for (var i = _currentAvailableIdsPointer; i > 0; i--)
                {
                    var id = _availableIds[_currentAvailableIdsPointer];
                    var entity = _entities[_currentId];
                    _entities[id] = entity;
                    // TODO: do it for components too.
                    UnsetEntity(entity.Id);
                    
                    // This is a little bit counter-intuitive: when I destroy an entity, I don't decrement this counter.
                    // Instead, this is done on finalization (now).
                    _entitiesCount--;
                    
                    _currentId--;
                    _currentAvailableIdsPointer--;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UnsetEntity(int index)
        {
            _entities[index].Id = 0;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureEntitiesCapacity(int id)
        {
            if (id >= _entities.Length)
            {
                lock(_entities)
                    if (id >= _entities.Length)
                        Array.Resize(ref _entities, _entities.Length * 2);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureIdentifiersCapacity()
        {
            if (_currentAvailableIdsPointer >= _availableIds.Length)
            {
                lock(_availableIds)
                    Array.Resize(ref _availableIds, _availableIds.Length * 2);
            }
        }
    }
}