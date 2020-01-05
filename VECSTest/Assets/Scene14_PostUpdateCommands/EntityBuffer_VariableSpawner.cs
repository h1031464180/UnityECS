using System;
using Unity.Entities;

[Serializable]
public struct EntityBuffer_VariableSpawner : IComponentData
{
    public Entity prefab;
}


/// <summary>
/// 发射的生成时间
/// </summary>
[Serializable]
public struct ProjectileSpawnTime : IComponentData
{
    public float SpawnTime;
}


