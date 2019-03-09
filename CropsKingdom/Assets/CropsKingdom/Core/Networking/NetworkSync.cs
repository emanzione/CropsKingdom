using GameLoop.Networking.Buffers;
using UnityEngine;

namespace CropsKingdom.Core.Networking
{
    public class NetworkSync : MonoBehaviour
    {
        private NetworkEntity _entity;
        private Vector3 _oldPosition;
        private byte _tag = (byte) NetworkMessageTag.EntityTransformSync;
        
        protected void Start()
        {
            _entity = GetComponent<NetworkEntity>();
            _oldPosition = transform.position;
        }

        protected void Update()
        {
            if (transform.position != _oldPosition)
            {
                var writer = default(NetworkWriter);
                writer.Initialize(NetworkBootstrapper.Instance.MemoryPool, 17);
                
                writer.Write(_tag);
                writer.Write(_entity.Id);
                
                var position = transform.position;
                writer.Write(position.x);
                writer.Write(position.y);
                writer.Write(position.z);
                
                NetworkBootstrapper.Instance.SendAll(ref writer);
            }
        }
    }
}