using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;

public class PureJob_Main : MonoBehaviour
{
    public int createCount = 300;
    public GameObject prefab;                       // 雨滴预制体
    public int dropSpeed = 10;
    public int dropHeight = 50;
    public int insRnage = 40;                           // 生成范围

    private EntityManager entityManager;

    private NativeArray<Entity> entities;

    void Start()
    {
        this.CreateMain();
    }
    private void CreateMain()
    {
        // 创建预制体实体
        entityManager = World.Active.EntityManager;

        entities = new NativeArray<Entity>(createCount, Allocator.Persistent);

        Entity entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefab, World.Active);

        entityManager.Instantiate(entityPrefab, entities);

        //初始化数据

        for (int i = 0; i < createCount; i++)
        {
            entityManager.SetComponentData<PureJob_DropData>(entities[i], new PureJob_DropData
            {
                delayTime = UnityEngine.Random.Range(0, 10),
                velocity = UnityEngine.Random.Range(dropSpeed / 10, dropSpeed),
            });

            var num = UnityEngine.Random.insideUnitSphere* insRnage;
            num.y = dropHeight;
            entityManager.SetComponentData<Translation>(entities[i], new Translation
            {
                Value = num
            });
        }

    }


    private void OnDestroy()
    {
        entities.Dispose();
    }
}
