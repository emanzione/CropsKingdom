using System;
using System.Collections.Generic;
using ENet;
using GameLoop.Networking.Buffers;
using GameLoop.Networking.Memory;
using UnityEngine;

namespace CropsKingdom.Core.Networking
{
    public class NetworkServerManager : MonoBehaviour
    {
        public static NetworkServerManager Instance;
        public IMemoryPool MemoryPool;
        private NetworkServer _server;

        public event EventHandler<Peer> OnConnectionEvent;
        public event EventHandler<Peer> OnDisconnectionEvent;
        
        protected void Awake()
        {
            var allocator = new SimpleManagedAllocator();
            MemoryPool = new SimpleMemoryPool(allocator);
            Instance = this;
            _server = NetworkManager.InitializeServer(50000);
            _server.OnConnection = OnConnection;
            _server.OnDisconnection = OnDisconnection;
            _server.OnData = OnData;
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
            OnDisconnectionEvent?.Invoke(this, peer);
        }

        private void OnConnection(Peer peer)
        {
            Debug.Log("Connection from " + peer.IP + ":" + peer.Port.ToString());
            OnConnectionEvent?.Invoke(this, peer);
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