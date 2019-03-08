using CropsKingdom.Core.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace CropsKingdom.Core.Systems
{
    public class FactionSpawnerSystem : JobComponentSystem
    {
        [BurstCompile]
        public struct FactionSpawnerJob : IJobProcessComponentData<FactionSpawner>
        {
            public void Execute([ReadOnly] ref FactionSpawner data)
            {
                throw new System.NotImplementedException();
            }
        }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            throw new System.NotImplementedException();
        }
    }
}