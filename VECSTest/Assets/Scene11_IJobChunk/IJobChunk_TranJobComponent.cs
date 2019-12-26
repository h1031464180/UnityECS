using UnityEngine;
using UnityEngine.Jobs;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Collections;
using Unity.Burst;
[DisableAutoCreation]
public class IJobChunk_TranJobComponent : JobComponentSystem
{
    EntityQuery entityQuery;
    protected override void OnCreate()
    {
        entityQuery = this.GetEntityQuery(typeof(Translation), typeof(IJobChunk_DropData));
    }
    [BurstCompile]
    struct TranslateJob : IJobChunk
    {
        [ReadOnly]
        public float deltaTime;
        [ReadOnly]
        public int minHeight;
        [ReadOnly]
        public float velocity;
        [ReadOnly]
        public float delay;
        public ArchetypeChunkComponentType<Translation> archetypeTranslation;
        public ArchetypeChunkComponentType<IJobChunk_DropData> archetypeDropData;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {

            var tranArray = chunk.GetNativeArray<Translation>(archetypeTranslation);
            var dropArray = chunk.GetNativeArray<IJobChunk_DropData>(archetypeDropData);

            for (int i = 0; i < chunk.Count; i++)
            {
                var dropData = dropArray[i];
                var translation = tranArray[i];
                if (dropData.delay > 0)
                {
                    dropData.delay -= deltaTime;
                }
                else
                {
                    if (translation.Value.y < minHeight)
                    {
                        translation.Value.y = 0;
                        dropData.velocity = velocity;
                        dropData.delay = delay;
                    }
                    else
                    {
                        translation.Value.y -= dropData.velocity * deltaTime;
                    }
                }
                dropArray[i] = dropData;
                tranArray[i] = translation;
            }

        }
    }


    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var tranArchetype = this.GetArchetypeChunkComponentType<Translation>();
        var dropArachetype = this.GetArchetypeChunkComponentType<IJobChunk_DropData>();
        
        TranslateJob translateJob = new TranslateJob()
        {
            deltaTime = Time.deltaTime,
            minHeight = -70,
            archetypeTranslation = tranArchetype,
            archetypeDropData = dropArachetype,
            delay = UnityEngine.Random.Range(1, 10),
            velocity = UnityEngine.Random.Range(1, 10),
        };
        //Debug.Log("---===="+ translateJob.velocity);
        JobHandle outputDeps = translateJob.Schedule(entityQuery, inputDeps);
        return outputDeps;
    }


}
