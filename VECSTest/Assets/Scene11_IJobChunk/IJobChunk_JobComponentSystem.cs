using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;

[DisableAutoCreation]
public class IJobChunk_JobComponentSystem : JobComponentSystem
{
    private EntityQuery m_group;        // 查询到特定组件的实体,将其放入这个组中
    protected override void OnCreate()
    {
        m_group = this.GetEntityQuery(typeof(Rotation), ComponentType.ReadOnly<IJobChunk_RotateData>());
    }

    struct RotateJob : IJobChunk
    {
        public float deltaTime;
        public ArchetypeChunkComponentType<Rotation> rotationType;                              // 原型组件类型
        [ReadOnly]
        public ArchetypeChunkComponentType<IJobChunk_RotateData> rotateDataType;  // 只读 原型组件类型


        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var chunkRotations = chunk.GetNativeArray(rotationType);
            var chunkRotateDatas = chunk.GetNativeArray(rotateDataType);

            for (int i = 0; i < chunk.Count; i++)
            {
                var rotation = chunkRotations[i];
                var rotateSpeed = chunkRotateDatas[i];

                chunkRotations[i] = new Rotation()
                {
                    Value = math.mul(
                        math.normalize(rotation.Value),
                        quaternion.AxisAngle(math.up(), rotateSpeed.value * deltaTime))
                };
            }



        }
    }


    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var rotationType = GetArchetypeChunkComponentType<Rotation>();
        var rotateType = GetArchetypeChunkComponentType<IJobChunk_RotateData>(true);



        RotateJob rotateJob = new RotateJob
        {
            deltaTime = Time.deltaTime,
            rotationType = rotationType,
            rotateDataType = rotateType,
        };
        JobHandle jobHandle = rotateJob.Schedule(m_group, inputDeps);
        return jobHandle;
    }


}
