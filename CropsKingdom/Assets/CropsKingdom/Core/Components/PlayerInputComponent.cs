using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace CropsKingdom.Core.Components
{
    public struct PlayerInputComponent : IComponentData
    {
        public byte IsLeftClick;
        public byte IsRightClick;
        public float3 MousePosition;
    }
}