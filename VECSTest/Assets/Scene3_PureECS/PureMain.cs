﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Rendering;
using Unity.Transforms;

public class PureMain : MonoBehaviour
{
    public int entityCount = 1000;
    public int entityRange = 100;
    public Mesh mymesh;
    public Material mymaterial;
    void Start()
    {
        this.CreatePureMain();
    }

    void CreatePureMain()
    {
        #region  1.创建本地实体数组
        // 世界自动创建的唯一实体管理器
        EntityManager entityManager = World.Active.EntityManager;

        // 创建一个原型模型（可以类比   GameObject.CreatePrimitive ）, 函数参数代表该实体有哪些组件（注意要想实体在场景中显示必须有 Translation，RenderMesh,LoalToWorld这三个组件）
        EntityArchetype entityArchetype = entityManager.CreateArchetype
        (
            typeof(Translation),
            typeof(RenderMesh),
            ComponentType.ReadWrite<LocalToWorld>()                     // 实体的矩阵,
        );

        NativeArray<Entity> entities = new NativeArray<Entity>(entityCount, Allocator.Temp);

        entityManager.CreateEntity(entityArchetype, entities);

        #endregion

        #region 2.初始化数组中的实体数据

        for (int i = 0; i < entityCount; i++)
        {
            Translation translation = new Translation();
            translation.Value = Random.insideUnitSphere * entityRange;                      // 设置位置为随机的球体
            entityManager.SetComponentData<Translation>(entities[i], translation);  // 设置位置信息

            // 给所有实体 设置网格 和材质 信息 这些UnityECS做了优化处理，使用共享网格材质的函数进行添加
            entityManager.SetSharedComponentData<RenderMesh>(entities[i], new RenderMesh { mesh = mymesh, material = mymaterial });
        }

        entities.Dispose();     // 这里把内存缓冲区释放掉
        #endregion
    }
}
