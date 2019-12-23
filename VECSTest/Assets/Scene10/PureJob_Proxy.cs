using UnityEngine;
using Unity.Entities;

public class PureJob_Proxy : MonoBehaviour, IConvertGameObjectToEntity
{
    public PureJob_DropData dropData;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData<PureJob_DropData>(entity, dropData);
    }


}
