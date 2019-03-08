using CropsKingdom.Core.Components;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Burst;
using UnityEngine;

namespace CropsKingdom.Core.Systems
{
    public class PlayerInputSystem : JobComponentSystem
    {
        [BurstCompile]
        private struct PlayerInputJob : IJobProcessComponentData<PlayerInputComponent>
        {
            public byte IsLeftClick;
            public byte IsRightClick;
            public float3 MousePosition;
            
            public void Execute(ref PlayerInputComponent data)
            {
                data.IsLeftClick = IsLeftClick;
                data.IsRightClick = IsRightClick;
                data.MousePosition = MousePosition;
            }
        }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var mousePosition = Input.mousePosition;
            var ray = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out var hitInfo))
            {
                mousePosition = new float3(hitInfo.point.x, 1, hitInfo.point.z);
            }
            
            var job = default(PlayerInputJob);
            job.IsLeftClick = (byte)((Input.GetMouseButtonDown(0)) ? 1 : 0);
            job.IsRightClick = (byte)((Input.GetMouseButtonDown(1)) ? 1 : 0);
            job.MousePosition = mousePosition;

            return job.Schedule(this, inputDeps);
        }
    }
}