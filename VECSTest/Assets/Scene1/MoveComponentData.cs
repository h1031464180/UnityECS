using Unity.Entities;
using Unity.Mathematics;

// 可序列化一定要加
[System.Serializable]
public struct MoveComponentData : IComponentData
{
    // 注意： ECS框架数据都是用结构体进行封装。
    // 类似 unity中的vector3这个类型 unity这里单独把他封装到了 Unity.Mathematics, 可以进入到命名空间中看它的定义封装了很多数据类型。
    public float3 vector3;
}
