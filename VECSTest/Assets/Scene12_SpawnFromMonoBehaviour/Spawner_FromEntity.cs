using Unity.Entities;

public struct Spawner_FromEntity : IComponentData
{
    public float time;
    public Entity Prefab;
}
