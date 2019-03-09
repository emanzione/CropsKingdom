using Unity.Entities;
using Unity.Transforms;

namespace CropsKingdom.Core.Components
{
    public struct FactionSpawner : ISharedComponentData
    {
        
    }
    
    public class FactionSpawnerProxy : SharedComponentDataProxy<FactionSpawner> {}
}