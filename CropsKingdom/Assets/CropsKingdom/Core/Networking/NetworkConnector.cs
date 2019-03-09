using System.Collections.Concurrent;
using System.Collections.Generic;
using ENet;
using GameLoop.Networking.Buffers;
using UnityEngine;

namespace CropsKingdom.Core.Networking
{
    public class NetworkConnector : MonoBehaviour
    {
        public NetworkRemoteEntity BuildingPrefab;
        public NetworkRemoteEntity UnitPrefab;
        
        private NetworkClient _client;
        private Dictionary<int, NetworkRemoteEntity> _entities;
        
        private void Start()
        {
            _entities = new Dictionary<int, NetworkRemoteEntity>();
            _client = NetworkManager.InitializeClient("127.0.0.1", 50000);
            _client.OnConnection = OnConnection;
            _client.OnDisconnection = OnDisconnection;
            _client.OnData = OnData;
        }
        
        private void OnData(Peer peer, byte[] data)
        {
            Debug.Log("Received data (" + data.Length.ToString() + ") from server.");

            var reader = default(NetworkReader);
            reader.Initialize(ref data);

            var tag = (NetworkMessageTag)reader.ReadByte();

            switch (tag)
            {
                case NetworkMessageTag.EntitySpawn:
                    {
                        var id = reader.ReadInt();
                        var type = (EntityType) reader.ReadByte();
                        var position = new Vector3(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat());
    
                        NetworkRemoteEntity entity;
                        switch (type)
                        {
                            case EntityType.Unit:
                                entity = GameObject.Instantiate<NetworkRemoteEntity>(UnitPrefab);
                                break;
                            case EntityType.Building:
                                entity = GameObject.Instantiate<NetworkRemoteEntity>(BuildingPrefab);
                                break;
                            default:
                                return;
                        }
    
                        entity.Id = id;
                        entity.transform.position = position;
    
                        _entities.Add(id, entity);
                    }
                    break;
                case NetworkMessageTag.EntityTransformSync:
                    {
                        var id = reader.ReadInt();
                        var position = new Vector3(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat());

                        NetworkRemoteEntity entity = _entities[id];
                        entity.transform.position = position;
                    }
                    break;
            }
        }

        private void OnDisconnection(Peer peer)
        {
            Debug.Log("Disconnection from server.");
        }

        private void OnConnection(Peer peer)
        {
            Debug.Log("Connection established.");
        }
        
        private void Update()
        {
            _client.Poll();
        }

        private void OnDestroy()
        {
            _client.Disconnect();
            NetworkManager.Deinitialize();
        }
    }
}