using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Collections;

public class PureJob_DropJobSystem : JobComponentSystem
{

    // Unity Job提供的获取实体组件的方法

    struct GravityJob : IJobForEach<PureJob_DropData, Translation>
    {
        [ReadOnly]
        public int minHeight;
        [ReadOnly]
        public float deltaTime;
        [ReadOnly]
        public int velocity;
        [ReadOnly]
        public int delay;
        public void Execute(ref PureJob_DropData dropData, ref Translation translation)
        {
            if (dropData.delayTime > 0)
            {
                dropData.delayTime -= deltaTime;
            }
            else
            {
                if (translation.Value.y < minHeight)
                {
                    translation.Value.y = 0;
                    dropData.velocity = velocity;
                    dropData.delayTime = delay;
                }
                else
                {
                    translation.Value.y -= dropData.velocity * deltaTime;
                }
            }
        }
    }


    public int minHeight = -100;                              // 下落最低值
    #region
    /*
    protected override void OnUpdate()
    {
        // 通过ECS提供的筛方式遍历场景中包含该数据组件的实体
        Entities.ForEach((ref Translation translation, ref DropData dropData) =>
        {

            if (dropData.delay > 0)
            {
                dropData.delay -= Time.deltaTime;
            }
            else
            {
                if (translation.Value.y < minHeight)
                {
                    translation.Value.y = 0;
                    dropData.velocity = Random.Range(1, 10);
                    dropData.delay = Random.Range(1, 10);
                }
                else
                {
                    translation.Value.y -= dropData.velocity * Time.deltaTime;
                }
            }
        });
    }
    */
    #endregion
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        GravityJob gravityJobSystem = new GravityJob
        {
            minHeight = minHeight,
            delay = UnityEngine.Random.Range(1, 10),
            velocity = UnityEngine.Random.Range(1, 10) * 10,
            deltaTime = Time.deltaTime
        };
        JobHandle jobHandle = gravityJobSystem.Schedule(this, inputDeps);
        jobHandle.Complete();
        return jobHandle;
    }


}
