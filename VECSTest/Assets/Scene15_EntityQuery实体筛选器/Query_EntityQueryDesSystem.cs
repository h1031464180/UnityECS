using Unity.Entities;
using Unity.Jobs;

//对于筛选更为复杂得到实体，或 且 取反~~ 可以使用EntityQueryDes 筛选器
public class Query_EntityQueryDesSystem : JobComponentSystem
{
    private EntityQuery entityQuery;


    protected override void OnCreate()
    {
        //-- 第一种
        var query = new EntityQueryDesc()
        {
            All = new ComponentType[] { typeof(ComponentA), typeof(ComponentB), typeof(ComponentC) }
        };
        //代表要筛选同时包含这个数组里所有类型的组件的实体。
        this.entityQuery = GetEntityQuery(query);

        //-- 第二种
        var twoQuery = new EntityQueryDesc()
        {
            All = new ComponentType[] { typeof(ComponentA) },
            Any = new ComponentType[] { typeof(ComponentB), typeof(ComponentC) },
            None = new ComponentType[] { typeof(ComponentD) }
        };
        // 筛选必须包含A组件 ，包含 ComponentB或者C组件，不包含D组件的实体！

    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        return inputDeps;
    }
}
