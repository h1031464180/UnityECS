using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class EntityBuffer_VariableSpawnerSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity spawnerEntity, ref EntityBuffer_VariableSpawner spawnerData, ref Translation translation) =>
        {
            var spawnTime = Time.deltaTime;
            var newEntity = PostUpdateCommands.Instantiate(spawnerData.prefab);         // 动态创建实体  筛选器内
            PostUpdateCommands.AddComponent<Parent>(newEntity, new Parent
            {
                Value = spawnerEntity
            });
            PostUpdateCommands.AddComponent<LocalToParent>(newEntity, new LocalToParent());
            PostUpdateCommands.SetComponent<Translation>(newEntity, new Translation { Value = new float3(0, 0.3f * math.sin(5.0f * spawnTime), 0) });
        });
    }


}
