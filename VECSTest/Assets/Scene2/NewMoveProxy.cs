using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[RequiresEntityConversion]  // 添加特性
public class NewMoveProxy : MonoBehaviour, IConvertGameObjectToEntity
{
    // 这里将参数暴露给外部 方便初始化值
    public float3 Vector3;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var data = new MoveComponentData();
        data.vector3 = Vector3;
        dstManager.AddComponentData<MoveComponentData>(entity, data);
        
        // 通过此方式可方便给该实体添加 数据组件并初始化

    }

}
