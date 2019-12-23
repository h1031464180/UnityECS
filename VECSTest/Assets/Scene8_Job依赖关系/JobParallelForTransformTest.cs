using UnityEngine;
using Unity.Jobs;
using Unity.Entities;
using Unity.Collections;
using UnityEngine.Jobs;

public class JobParallelForTransformTest : MonoBehaviour
{
    struct VelocityJob : IJobParallelFor
    {
        public NativeArray<Vector3> positions;
        public NativeArray<Vector3> velocitys;
        public float delaTime;
        // 并行化 让一个线程做数组的一部分处理
        public void Execute(int index)
        {
            positions[index] = positions[index] + velocitys[index] * delaTime;
        }
    }

    // 注意这个 job扩展是定义在 UnityEngine.Jobs命名空间下
    struct ApplyTransform : IJobParallelForTransform
    {
        public NativeArray<Vector3> positions;
        public void Execute(int index, TransformAccess transform)
        {
            transform.position = positions[index];
        }
    }

    public int gameCount = 300;
    public GameObject prefab;
    public GameObject[] gameObjs;
    private TransformAccessArray tranAccessArray;      // 类似 nativeContainer 也是需要释放                     
    void Start()
    {
        gameObjs = new GameObject[gameCount];
        tranAccessArray = new TransformAccessArray(gameCount);           // 注意 这种类型数组必须将capacity主动填上  不能像list一样直接new便可以add
        for (int i = 0; i < gameCount; i++)
        {
            gameObjs[i] = Instantiate<GameObject>(prefab);
            gameObjs[i].transform.position = UnityEngine.Random.insideUnitSphere * 40;
            tranAccessArray.Add(gameObjs[i].transform);
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

        //依赖按照速度计算的到的位置数组
        ApplyTransform applyTransform = new ApplyTransform()
        {
            positions = tmpPositions
        };

        // 2.执行  
        //信号量 主线程如何知道子线程执行完毕    gameCount 指定总共子线程执行数据数量 10：每个子线程以下处理多少次
        JobHandle jobHandle = job.Schedule(gameCount, 10);


        JobHandle tranHandle = applyTransform.Schedule(tranAccessArray, jobHandle);

        // 3.同步
        jobHandle.Complete();
        tranHandle.Complete();

        //4。更新位置
        //for (int i = 0; i < gameCount; i++)
        //{
        //    gameObjs[i].transform.position = tmpPositions[i];
        //}


        tmpPositions.Dispose();
        tmpVelocitys.Dispose();
    }

    private void OnDestroy()
    {
        this.tranAccessArray.Dispose();
    }
}
