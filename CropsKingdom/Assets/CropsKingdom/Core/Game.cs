using System.Collections.Generic;
using CropsKingdom.Core.Networking;
using CropsKingdom.Core.Players.Factions;
using ENet;
using GameLoop.Networking.Buffers;
using GameLoop.Networking.Memory;

namespace CropsKingdom.Core
{
    public class Game
    {
        public int Id;
        private List<FactionSpawner> _factionSpawners = new List<FactionSpawner>();
        private List<NetworkEntity> _entities = new List<NetworkEntity>();

        public Game()
        {
            NetworkServerManager.Instance.OnConnectionEvent +=
                (sender, peer) => SendInitialState(SimpleMemoryPool.Instance, peer);
        }
        
        public void RegisterFactionSpawner(FactionSpawner spawner)
        {
            _factionSpawners.Add(spawner);
        }
        
        public void SpawnPlayer()
        {
            
        }
        
        public void RegisterEntity(NetworkEntity entity)
        {
            entity.Id = _entities.Count;
            _entities.Add(entity);
        }

        public void SendInitialState(IMemoryPool memoryPool, Peer peer)
        {
            foreach (var entity in _entities)
            {
                var writer = default(NetworkWriter);
                writer.Initialize(memoryPool, 18);
                
                writer.Write((byte)NetworkMessageTag.EntitySpawn);
                writer.Write(entity.Id);
                writer.Write((byte)entity.Type);
                
                var position = entity.transform.position;
                writer.Write(position.x);
                writer.Write(position.y);
                writer.Write(position.z);

                var packet = new Packet();
                packet.Create(writer.GetBuffer());
                peer.Send(0, ref packet);
                packet.Dispose();
            }
        }
    }
}