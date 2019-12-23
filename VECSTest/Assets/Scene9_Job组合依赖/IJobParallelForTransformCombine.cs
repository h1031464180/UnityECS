using UnityEngine;
using Unity.Jobs;
using Unity.Entities;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Burst;

public class IJobParallelForTransformCombine : MonoBehaviour
{
    [BurstCompile]
    struct VelocityJob : IJobParallelFor
    {
        public NativeArray<Vector3> positions;
        [ReadOnly]
        public NativeArray<Vector3> velocitys;
        public float delaTime;
        // 并行化 让一个线程做数组的一部分处理
        public void Execute(int index)
        {
            positions[index] = positions[index] + velocitys[index] * delaTime;
        }
    }
    [BurstCompile]
    // 物体旋转
    struct RotateJob : IJobParallelFor
    {
        public NativeArray<quaternion> quaternions;
        [ReadOnly]
        public float deltaTime;
        public void Execute(int index)
        {
            quaternions[index] = math.mul(math.normalize(quaternions[index]), quaternion.AxisAngle(math.up(), 5 * deltaTime));
        }
    }

    [BurstCompile] 
    // 注意这个 job扩展是定义在 UnityEngine.Jobs命名空间下
    struct ApplyTransform : IJobParallelForTransform
    {
        [ReadOnly]
        public NativeArray<Vector3> positions;
        [ReadOnly]
        public NativeArray<quaternion> quaternions;
        public void Execute(int index, TransformAccess transform)
        {
            transform.position = positions[index];
            transform.rotation = quaternions[index];
        }
    }

    public int gameCount = 300;
    public GameObject prefab;
    public GameObject[] gameObjs;
    private TransformAccessArray tranAccessArray;
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


        tmpPositions = new NativeArray<Vector3>(gameCount, Allocator.Persistent);
        tmpVelocitys = new NativeArray<Vector3>(gameCount, Allocator.Persistent);
        tmpQuaternion = new NativeArray<quaternion>(gameCount, Allocator.Persistent);

    }
    NativeArray<Vector3> tmpPositions;
    NativeArray<Vector3> tmpVelocitys;
    NativeArray<quaternion> tmpQuaternion;

    void Update()
    {
        // 1.准备数据

        for (int i = 0; i < gameCount; i++)
        {
            tmpVelocitys[i] = new Vector3(0, 1, 0);
            //tmpPositions[i] = tmpPositions[i] + tmpVelocitys[i] * Time.deltaTime;
            tmpPositions[i] = gameObjs[i].transform.position;
            tmpQuaternion[i] = gameObjs[i].transform.rotation;
        }
        VelocityJob job = new VelocityJob()
        {
            positions = tmpPositions,
            delaTime = Time.deltaTime,
            velocitys = tmpVelocitys
        };
        RotateJob rotateJob = new RotateJob()
        {
            deltaTime = Time.deltaTime,
            quaternions = tmpQuaternion
        };

        //依赖按照速度计算的到的位置数组
        ApplyTransform applyTransform = new ApplyTransform()
        {
            positions = tmpPositions,
            quaternions = tmpQuaternion
        };

        // 2.执行  
        //信号量 主线程如何知道子线程执行完毕    gameCount 指定总共子线程执行数据数量 10：每个子线程以下处理多少次
        JobHandle jobHandle = job.Schedule(gameCount, 10);                  // 移动 Job

        JobHandle rotateHandle = rotateJob.Schedule(gameCount, 10);     // 旋转 Job

        JobHandle combineHandle = JobHandle.CombineDependencies(jobHandle, rotateHandle);   // 赋值job 共同依赖联合的句柄~~

        JobHandle tranHandle = applyTransform.Schedule(tranAccessArray, combineHandle); // 对 obj 赋值 job

        // 3.同步
        jobHandle.Complete();
        rotateHandle.Complete();
        tranHandle.Complete();

        //4。更新位置
        //for (int i = 0; i < gameCount; i++)
        //{
        //    gameObjs[i].transform.position = tmpPositions[i];
        //}


       
    }

    private void OnDestroy()
    {
        this.tmpPositions.Dispose();
        this.tmpQuaternion.Dispose();
        this.tmpVelocitys.Dispose();
        this.tranAccessArray.Dispose();
    }
       
     
}
