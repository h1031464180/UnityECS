using Unity.Entities;
using Unity.Mathematics;
using System;
using Unity.Transforms;

namespace Scene15
{
    [Serializable]
    public struct PlayerInput : IComponentData
    {
        public float3 Vector;
    }
    [Serializable]
    public struct PlayerData : IComponentData
    {
        public float moveSpeed;
    }
    [Serializable]
    public struct CameraData : IComponentData
    {
        public float time;
    }
}