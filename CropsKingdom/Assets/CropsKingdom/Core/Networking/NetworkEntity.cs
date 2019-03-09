using UnityEngine;

namespace CropsKingdom.Core.Networking
{
    public class NetworkEntity : MonoBehaviour
    {
        public EntityType Type;
        public int Id;

        private void Start()
        {
            NetworkBootstrapper.Instance.RegisterEntity(this);
        }
    }
}