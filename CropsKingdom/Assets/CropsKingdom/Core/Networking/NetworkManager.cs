using System;
using System.Collections.Concurrent;
using System.Net;
using ENet;

namespace CropsKingdom.Core.Networking
{
    public struct NetworkServer
    {
        public Host Host;
        public ConcurrentDictionary<uint, Peer> Peers;

        public Action<Peer> OnConnection;
        public Action<Peer> OnDisconnection;
        public Action<Peer, byte[]> OnData;

        /*public NetworkServer()
        {
            Peers = new ConcurrentDictionary<uint, Peer>();
        }*/
    
        public void Poll()
        {
            while (Host.Service(0, out Event netEvent) > 0)
            {
                switch (netEvent.Type)
                {
                    case EventType.Receive:
                        var buffer = new byte[netEvent.Packet.Length];
                        netEvent.Packet.CopyTo(buffer);
                        HandleData(netEvent.Peer, buffer);
                        break;
                    case EventType.Connect:
                        HandleNewConnection(netEvent.Peer);
                        break;
                    case EventType.Disconnect:
                        HandleDisconnection(netEvent.Peer);
                        break;
                    case EventType.Timeout:
                        HandleDisconnection(netEvent.Peer);
                        break;
                    case EventType.None:
                        break;
                }
            }
        }
    
        public void Send(Peer peer, byte[] data, byte channel, PacketFlags flags)
        {
            var packet = default(Packet);
            packet.Create(data, flags);

            peer.Send(channel, ref packet);
        }

        public void DisconnectAll()
        {
            foreach (var peer in Peers)
            {
                peer.Value.Disconnect(0);
            }
        }

        public void Close()
        {
            DisconnectAll();
            Host.Dispose();
        }
    
        private void HandleData(Peer peer, byte[] data)
        {
            OnData?.Invoke(peer, data);
        }
    
        private void HandleNewConnection(Peer peer)
        {
            Peers.TryAdd(peer.ID, peer);
            OnConnection?.Invoke(peer);
        }

        private void HandleDisconnection(Peer peer)
        {
            Peers.TryRemove(peer.ID, out var peeeeer);
            OnDisconnection?.Invoke(peer);
        }
    }

    public struct NetworkClient
    {
        public Host Host;
        public Peer Peer;
    
        public Action<Peer> OnConnection;
        public Action<Peer> OnDisconnection;
        public Action<Peer, byte[]> OnData;
    
        public void Poll()
        {
            while (Host.Service(0, out Event netEvent) > 0)
            {
                switch (netEvent.Type)
                {
                    case EventType.Receive:
                        var buffer = new byte[netEvent.Packet.Length];
                        netEvent.Packet.CopyTo(buffer);
                        HandleData(buffer);
                        break;
                    case EventType.Connect:
                        HandleConnection();
                        break;
                    case EventType.Disconnect:
                        HandleDisconnection();
                        break;
                    case EventType.Timeout:
                        HandleDisconnection();
                        break;
                    case EventType.None:
                        break;
                }
            }
        }
    
        public void Send(byte[] data, byte channel, PacketFlags flags)
        {
            var packet = default(Packet);
            packet.Create(data, flags);

            Peer.Send(channel, ref packet);
        }

        public void Disconnect()
        {
            Peer.Disconnect(0);
            Host.Dispose();
        }
    
        private void HandleData(byte[] data)
        {
            OnData?.Invoke(Peer, data);
        }
    
        private void HandleConnection()
        {
            OnConnection?.Invoke(Peer);
        }

        private void HandleDisconnection()
        {
            OnDisconnection?.Invoke(Peer);
        }
    }

    public static class NetworkManager
    {
        private static bool _isInitialized = false;
    
        private static void Initialize()
        {
            if(_isInitialized) return;
            ENet.Library.Initialize();
            _isInitialized = true;
        }

        public static NetworkServer InitializeServer(ushort port)
        {
            Initialize();

            var netPeer = new NetworkServer();
            netPeer.Peers = new ConcurrentDictionary<uint, Peer>();
            netPeer.Host = new Host();
            var address = new Address(){Port = port};
            netPeer.Host.Create(address, (int)ENet.Library.maxPeers);
        
            return netPeer;
        }

        public static NetworkClient InitializeClient(string endpoint, ushort port)
        {
            Initialize();
        
            var netPeer = new NetworkClient();
            netPeer.Host = new Host();
            var address = new Address { Port = port };
            address.SetHost(endpoint);
            netPeer.Host.Create();

            netPeer.Peer = netPeer.Host.Connect(address);

            return netPeer;
        }

        public static void Deinitialize()
        {
            if (!_isInitialized) return;
            ENet.Library.Deinitialize();
            _isInitialized = false;
        }
    }
}