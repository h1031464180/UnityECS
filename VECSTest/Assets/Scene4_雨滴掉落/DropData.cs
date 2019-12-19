using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[System.Serializable]
public struct DropData : IComponentData
{
    public float delay;
    public float velocity;
}
