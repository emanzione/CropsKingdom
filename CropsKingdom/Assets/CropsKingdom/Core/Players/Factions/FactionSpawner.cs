using CropsKingdom.Core.Networking;
using UnityEngine;

namespace CropsKingdom.Core.Players.Factions
{
    public class FactionSpawner : MonoBehaviour
    {
        public NetworkEntity[] InitialEntities;

        private void Start()
        {
            //GameManager.RegisterFactionSpawner(this);
        }
    }
}