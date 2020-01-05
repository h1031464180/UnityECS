using Unity.Entities;
using Unity.Jobs;

public class Query_SetSharedComponentFilter : JobComponentSystem
{
    EntityQuery entityQuery;
    protected override void OnCreate()
    {
        entityQuery = GetEntityQuery(typeof(ShareComponentA));
        entityQuery.SetFilter<ShareComponentA>(new ShareComponentA { type = 1 });
        // 注意： 这里的实体上数据结构 是继承自 ISahredComponentData  
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        return inputDeps;
    }


}
