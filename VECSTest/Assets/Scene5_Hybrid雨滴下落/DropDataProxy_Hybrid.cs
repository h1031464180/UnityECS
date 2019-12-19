using UnityEngine;
using Unity.Entities;

public class DropDataProxy_Hybrid : MonoBehaviour,IConvertGameObjectToEntity
{
    public DropData dropData;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData<DropData>(entity, dropData);
    }
}
