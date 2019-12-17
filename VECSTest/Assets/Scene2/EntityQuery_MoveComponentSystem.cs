
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class EntityQuery_MoveComponentSystem : ComponentSystem
{
    EntityQuery EntityQuery;
    protected override void OnCreate()
    {
        // 将筛选条件进行创建
        EntityQuery = GetEntityQuery(typeof(Translation), ComponentType.ReadOnly<MoveComponentData>());
    }
    // 类似 Monobehaviour中的Update 只要继承 ComponentSystem， Entity框架就会自动执行OnUpdate
    protected override void OnUpdate()
    {
        // 类似Monobehaviour中的 GameObject 的查找方式（  遍历整个 World.Active 一个Scene一个World 通过匹配数据组件方式返回符合条件的 实体（Entity，我的个人猜想），
        Entities.With(EntityQuery).ForEach((ref Translation translation, ref MoveComponentData moveComponentData) =>
        {
            //这里可以想像，符合条件的实体已经被筛选出来了，我们要对起位置进行操作
            //我们要更改对应实体的 MoveComponentData 的数据
            moveComponentData.vector3 += new float3(0, 1, 0) * Time.deltaTime;
            //然后把更改的值赋值给
            translation.Value = moveComponentData.vector3;
        });

    }

}
