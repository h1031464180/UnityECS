using Unity.Entities;
using System;

namespace Scene15
{
    [Serializable]
    public struct SpawnerData : IComponentData
    {
        public int energyNum;
        public float colliderRadius;

    }
}