using Unity.Entities;
using Unity.Jobs;

public class Query_SetChangedVersionFilter : JobComponentSystem
{
    EntityQuery entityQuery;
    protected override void OnCreate()
    {
        entityQuery = GetEntityQuery(typeof(ComponentA), typeof(ComponentB));
        entityQuery.SetFilterChanged(typeof(ComponentA));
        // 筛选 指定该实体的哪个组件发生了改变
    }

    struct ChangeJob : IJobForEach<ComponentA>
    {
        public void Execute(ref ComponentA comA)
        {
            // 注意我在Job中并没有修改，但是这里也判定了 ComponentA被修改了
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        return inputDeps;
    }

}
