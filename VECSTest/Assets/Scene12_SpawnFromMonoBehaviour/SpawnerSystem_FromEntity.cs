using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

[UpdateInGroup(typeof(SimulationSystemGroup))]      // 标记更新组为模拟系统组
public class SpawnerSystem_FromEntity : JobComponentSystem
{

    /// 开始初始化实体命令缓存系统（BeginInitializationEntityCommandBufferSystem）被用来创建一个命令缓存，
    /// 这个命令缓存将在阻塞系统执行时被回放。虽然初始化命令在生成任务（SpawnJob）中被记录下来，
    /// 它并非真正地被执行（或“回放”）直到相应的实体命令缓存系统（EntityCommandBufferSystem）被更新。
    /// 为了确保transform系统有机会在新生的实体初次被渲染之前运行，SpawnerSystem_FromEntity将使用
    /// 开始模拟实体命令缓存系统（BeginSimulationEntityCommandBufferSystem）来回放其命令。
    /// 这就导致了在记录命令和初始化实体之间一帧的延迟，但是该延迟实际通常被忽略掉。
    /// </summary>

    BeginInitializationEntityCommandBufferSystem m_BeginInitializationEntityCommandBufferSystem;

    protected override void OnCreate()
    {
        ////在一个字段中缓存BeginInitializationEntityCommandBufferSystem，这样我们就不必在每一帧都创建它
        m_BeginInitializationEntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();

    }

    struct SpawnJob : IJobForEachWithEntity<Spawner_FromEntity, LocalToWorld>
    {
        public EntityCommandBuffer.Concurrent commandBuffer;
        public float delatime;
        public float3 position;

        public void Execute(Entity entity, int index, ref Spawner_FromEntity spawnerData, ref LocalToWorld location)
        {

            if (spawnerData.time <= 0)
            {
                spawnerData.time = 5;
                Debug.Log("创建一个cube");
                var instance = commandBuffer.Instantiate(index, spawnerData.Prefab);
                commandBuffer.SetComponent(index, instance, new Translation { Value = position });
                commandBuffer.SetComponent<Spawner_FromEntity>(index, instance, new Spawner_FromEntity { time = 5, Prefab = spawnerData.Prefab });
            }
            else
            {
                spawnerData.time -= delatime;
            }

            //commandBuffer.DestroyEntity(index, entity);
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        //取代直接执行结构的改变，一个任务可以添加一个命令到EntityCommandBuffer（实体命令缓存），从而在主线程上完成其任务后执行这些改变
        //命令缓存允许在工作线程上执行任何潜在消耗大的计算，同时把实际的增删排到之后把将要添加实例化命令到EntityCommandBuffer的任务加入计划
        var job = new SpawnJob
        {
            commandBuffer = m_BeginInitializationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
            delatime = Time.deltaTime,
            position = UnityEngine.Random.insideUnitSphere * 50
        };
        JobHandle jobHandle = job.Schedule(this, inputDeps);
        //Debug.Log("场景实体个数===" + EntityManager.EntityCapacity);   错误
        ///生成任务并行且没有同步机会直到阻塞系统执行
        ///当阻塞系统执行时，我们想完成生成任务，然后再执行那些命令（创建实体并放置到指定位置）
        /// 我们需要告诉阻塞系统哪个任务需要在它能回放命令之前完成
        m_BeginInitializationEntityCommandBufferSystem.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }


}
