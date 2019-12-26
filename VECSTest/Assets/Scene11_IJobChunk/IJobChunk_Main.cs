using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;

public class IJobChunk_Main : MonoBehaviour
{


    public int entityCount = 1000;
    public int entityRange = 100;
    public GameObject prefab;

    private EntityManager entityManager;
    private NativeArray<Entity> entityArray;
    void Start()
    {
        this.CreatePureMain();
    }

    void CreatePureMain()
    {

        entityManager = World.Active.EntityManager;

        entityArray = new NativeArray<Entity>(entityCount, Allocator.Persistent);

        Entity entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(this.prefab, World.Active);

        entityManager.Instantiate(entity, entityArray);

 
        for (int i = 0; i < entityCount; i++)
        {
            // 初始化位置信息
            Translation translation = new Translation();
            translation.Value = Random.insideUnitSphere * entityRange;                      // 设置位置为随机的球体
            translation.Value.y = 0;
            entityManager.SetComponentData<Translation>(entityArray[i], translation);  // 设置位置信息

            // 初始化延迟时间
            entityManager.AddComponentData<IJobChunk_DropData>(entityArray[i], new IJobChunk_DropData {  delay=5, velocity=10});

            
        }

    }

    private void OnDestroy()
    {
        entityArray.Dispose();     // 这里把内存缓冲区释放掉
    }

}
