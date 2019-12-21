using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public class IJobTest : MonoBehaviour
{
    struct VelocityJob : IJob
    {
        public NativeArray<Vector3> positions;
        public NativeArray<Vector3> velocitys;
        public float delaTime;
        public void Execute()
        {
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = positions[i] + velocitys[i] * delaTime;
            }
        }
    }
    public int gameCount = 300;
    public GameObject prefab;
    public GameObject[] gameObjs;
    void Start()
    {
        gameObjs = new GameObject[gameCount];
        for (int i = 0; i < gameCount; i++)
        {
            gameObjs[i] = Instantiate<GameObject>(prefab);
            gameObjs[i].transform.position = UnityEngine.Random.insideUnitSphere*40;
        }

    }


    void Update()
    {
        // 1.准备数据
        NativeArray<Vector3> tmpPositions = new NativeArray<Vector3>(gameCount, Allocator.TempJob);
        NativeArray<Vector3> tmpVelocitys = new NativeArray<Vector3>(gameCount, Allocator.TempJob);
        for (int i = 0; i < gameCount; i++)
        {
            tmpVelocitys[i] = new Vector3(0, 1, 0);
            //tmpPositions[i] = tmpPositions[i] + tmpVelocitys[i] * Time.deltaTime;
            tmpPositions[i] = gameObjs[i].transform.position;
        }
        VelocityJob job = new VelocityJob()
        {
            positions = tmpPositions,
            delaTime = Time.deltaTime,
            velocitys = tmpVelocitys
        };
        // 2.执行  
        //信号量 主线程如何知道子线程执行完毕
        JobHandle jobHandle = job.Schedule();

        // 3.同步
        jobHandle.Complete();

        for (int i = 0; i < gameCount; i++)
        {
            gameObjs[i].transform.position = tmpPositions[i];
        }


        tmpPositions.Dispose();
        tmpVelocitys.Dispose();
    }
}
