
using Unity.Entities;

public struct ComponentA : IComponentData
{
    public int type;
}
public struct ComponentB : IComponentData
{

}
public struct ComponentC : IComponentData
{

}

public struct ComponentD : IComponentData
{

}

public struct ShareComponentA : ISharedComponentData
{
    public int type;
}