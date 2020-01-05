using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public struct OtherComponentData : IComponentData
{
}

//共享组件
public struct ShareComponentData : ISharedComponentData
{
    public Mesh mesh;
}

public struct ChunkComponent : IComponentData
{

}

public class EntityNew : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {

        dstManager.AddChunkComponentData<ChunkComponent>(entity);
    }
}
// 状态组件
public struct SystemStateComponent : ISystemStateComponentData
{

}
//共享状态组件
public struct SharedSystemStateComponent : ISystemStateSharedComponentData
{

}
//可以被挂载多次的组件
public struct IBufferComponetData : IBufferElementData
{
    public int num;
}

public class BufferMonobehaviour : MonoBehaviour
{
    public GameObject prefab;
    private void Start()
    {
        var entityManager = World.Active.EntityManager;
        Entity entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(this.prefab, World.Active);
        entity = entityManager.Instantiate(entity);
        // 给实体添加一个缓冲队列
        DynamicBuffer<IBufferComponetData> dynamicBuffer = entityManager.AddBuffer<IBufferComponetData>(entity);
        //给该实体的缓冲队列添加数据
        dynamicBuffer.Add(new IBufferComponetData { num = 1 });
        dynamicBuffer.Add(new IBufferComponetData { num = 2 });
        dynamicBuffer.Add(new IBufferComponetData { num = 3 });

    }
}
//如何操作数据
public class BufferSystem : JobComponentSystem
{
    struct BufferJob : IJobForEachWithEntity_EB<IBufferComponetData>
    {
        public void Execute(Entity entity, int index, DynamicBuffer<IBufferComponetData> bufferDatas)
        {
            for (int i = 0; i < bufferDatas.Length; i++)
            {
                Debug.Log("===" + bufferDatas[i].num);
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        BufferJob bufferJob = new BufferJob();
        JobHandle bufferHandle = bufferJob.Schedule(this, inputDeps);
        return bufferHandle;
    }
}
