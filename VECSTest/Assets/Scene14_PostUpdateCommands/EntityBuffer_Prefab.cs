using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;


public class EntityBuffer_Prefab : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject projectlePrefab;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var spawnerData = new EntityBuffer_VariableSpawner
        {
            prefab = conversionSystem.GetPrimaryEntity(projectlePrefab)
        };
        dstManager.AddComponentData<EntityBuffer_VariableSpawner>(entity, spawnerData);
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(projectlePrefab);
    }
}
