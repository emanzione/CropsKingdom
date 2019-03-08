using Unity.Entities;
using Unity.Transforms;

namespace CropsKingdom.Core.Components
{
    public struct FactionSpawner : IComponentData
    {
        public Position SpawnPosition;
    }
    
    public class FactionSpawnerProxy : ComponentDataProxy<FactionSpawner> {}
}