using Unity.Entities;
using Unity.Jobs;

public class Query_GetEntityQuerySystem : JobComponentSystem
{
    private EntityQuery entityQuery;
    protected override void OnCreate()
    {
        // 通过得到只包含ComponentA和ComponentB组件实体的筛选器
        entityQuery = this.GetEntityQuery(typeof(ComponentA), typeof(ComponentB));

    }

    // IJobForEach也可以充当筛选器的作用，但是不如如上灵活方便
    struct EntityQueryJob : IJobForEach<ComponentA>
    {
        public void Execute(ref ComponentA a)
        {

        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        EntityQueryJob entityQueryJob = new EntityQueryJob()
        {

        };
        //讲这个值传入Job使用
        JobHandle jobHandle = entityQueryJob.Schedule(entityQuery, inputDeps);
        jobHandle.Complete();
        return jobHandle;
    }
}
