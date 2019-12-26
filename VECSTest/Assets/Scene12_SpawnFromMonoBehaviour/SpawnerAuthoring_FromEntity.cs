using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
// 将自己转换成实体，再由预设生成新的实体
[RequiresEntityConversion]                                             // IDeclareReferencedPrefabs接口的实现，声明引用的预设，好让转化系统提前知道它们的存在
public class SpawnerAuthoring_FromEntity : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    public GameObject prefab;

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(prefab);
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var spawnerData = new Spawner_FromEntity
        {   
            //被引用的预设因为做了声明将被转化成实体
            //所以我们这里只是将游戏对象标记到一个引用该预设的实体上
            Prefab = conversionSystem.GetPrimaryEntity(prefab),
            time = 5
        };
        dstManager.AddComponentData<Spawner_FromEntity>(entity, spawnerData);
    }


}
