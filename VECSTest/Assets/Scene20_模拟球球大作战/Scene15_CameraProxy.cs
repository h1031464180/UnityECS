using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
namespace Scene15
{
    public class Scene15_CameraProxy : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        public GameObject prefab;
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            Entity entity1 = conversionSystem.GetPrimaryEntity(prefab);
            dstManager.AddComponentData<CameraData>(entity1, new CameraData
            {
                time = 1
            });
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(prefab);
        }
    }
}