using Unity.Entities;

[System.Serializable]
public struct PureJob_DropData : IComponentData
{
    public int velocity;
    public float delayTime;
}
