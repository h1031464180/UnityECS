using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Rendering;
using Unity.Transforms;

public class Hybrid_DropMain : MonoBehaviour
{
    public int entityCount = 1000;
    public int entityRange = 100;
    public int delaySpeed = 1;
    public int dropSpeed = 1;
    public GameObject prefab;
    void Start()
    {
        this.CreatePureMain();
    }

    void CreatePureMain()
    {
        #region  1.创建本地实体数组
        // 世界自动创建的唯一实体管理器
        EntityManager entityManager = World.Active.EntityManager;

        NativeArray<Entity> entities = new NativeArray<Entity>(entityCount, Allocator.Temp);

        /*  这里不需要手动创建实体原型了
        // 创建一个原型模型（可以类比   GameObject.CreatePrimitive ）, 函数参数代表该实体有哪些组件（注意要想实体在场景中显示必须有 Translation，RenderMesh,LoalToWorld这三个组件）
        EntityArchetype entityArchetype = entityManager.CreateArchetype
        (
            typeof(Translation),
            typeof(RenderMesh),
            ComponentType.ReadWrite<LocalToWorld>(),                     // 实体的矩阵,
            ComponentType.ReadOnly<DropData>()                             // 
        );
        entityManager.CreateEntity(entityArchetype, entities);
        */

        // unity 以提供了一个转换实体的公共类 
        Entity entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(this.prefab, World.Active);
        // 然后对这个实体进行实例化 想象成 Instantiate
        entityManager.Instantiate(entity, entities);

        #endregion

        #region 2.初始化数组中的实体数据

        for (int i = 0; i < entityCount; i++)
        {
            // 初始化位置信息
            Translation translation = new Translation();
            translation.Value = Random.insideUnitSphere * entityRange;                      // 设置位置为随机的球体
            translation.Value.y = 0;
            entityManager.SetComponentData<Translation>(entities[i], translation);  // 设置位置信息

            // 初始化延迟时间
            entityManager.SetComponentData<DropData>(entities[i], new DropData { delay = Random.Range(1, 10), velocity = Random.Range(1, 100) });

            // 因为使用的预制体 这里不再需要手动设置网格信息
            // 给所有实体 设置网格 和材质 信息 这些UnityECS做了优化处理，使用共享网格材质的函数进行添加
            //entityManager.SetSharedComponentData<RenderMesh>(entities[i], new RenderMesh { mesh = mymesh, material = mymaterial });
        }

        entities.Dispose();     // 这里把内存缓冲区释放掉
        #endregion
    }
}
