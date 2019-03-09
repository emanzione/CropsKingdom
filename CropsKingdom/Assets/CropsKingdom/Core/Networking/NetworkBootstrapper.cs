using System.Collections.Generic;
using ENet;
using GameLoop.Networking.Buffers;
using GameLoop.Networking.Memory;
using UnityEngine;

namespace CropsKingdom.Core.Networking
{
    public class NetworkBootstrapper : MonoBehaviour
    {
        public static NetworkBootstrapper Instance;
        public IMemoryPool MemoryPool;
        private NetworkServer _server;
        private List<NetworkEntity> _entities;
        
        protected void Awake()
        {
            var allocator = new SimpleManagedAllocator();
            MemoryPool = new SimpleMemoryPool(allocator);
            _entities = new List<NetworkEntity>();
            Instance = this;
            _server = NetworkManager.InitializeServer(50000);
            _server.OnConnection = OnConnection;
            _server.OnDisconnection = OnDisconnection;
            _server.OnData = OnData;
        }

        public void RegisterEntity(NetworkEntity entity)
        {
            entity.Id = _entities.Count;
            _entities.Add(entity);
        }

        public void SendAll(ref NetworkWriter message)
        {
            Packet packet = new Packet();
            packet.Create(message.GetBuffer(), PacketFlags.None);
            foreach (var peer in _server.Peers)
            {
                peer.Value.Send(0, ref packet);
            }
            packet.Dispose();
        }
        
        private void OnData(Peer peer, byte[] data)
        {
            Debug.Log("Received data (" + data.Length.ToString() + ") from " + peer.IP + ":" + peer.Port.ToString());
        }

        private void OnDisconnection(Peer peer)
        {
            Debug.Log("Disconnection from " + peer.IP + ":" + peer.Port.ToString());
        }

        private void OnConnection(Peer peer)
        {
            Debug.Log("Connection from " + peer.IP + ":" + peer.Port.ToString());

            foreach (var entity in _entities)
            {
                var writer = default(NetworkWriter);
                writer.Initialize(MemoryPool, 18);
                
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

        private void Update()
        {
            _server.Poll();
        }

        private void OnDestroy()
        {
            _server.Close();
            NetworkManager.Deinitialize();
        }
    }
}