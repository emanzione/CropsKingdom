using CropsKingdom.Core.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace CropsKingdom.Core.Systems
{
    public class FactionSpawnerSystem : ComponentSystem
    {
        private ComponentGroup _componentGroup;
        
        protected override void OnCreateManager()
        {
            _componentGroup = GetComponentGroup(typeof(FactionSpawner), typeof(Position));
        }
        
        protected override void OnUpdate()
        {
            
        }
    }
}