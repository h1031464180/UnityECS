using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
[UpdateInGroup(typeof(InitializationSystemGroup))]
public class Enemy_Create : ComponentSystem
{
    protected override void OnUpdate()
    {
        throw new System.NotImplementedException();
    }

}

[UpdateInGroup(typeof(SimulationSystemGroup))]
public class Enemy_Move : ComponentSystem
{
    protected override void OnUpdate()
    {
        throw new System.NotImplementedException();
    }

}

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(Enemy_Move))]
public class Enemy_Attack : ComponentSystem
{
    protected override void OnUpdate()
    {
        throw new System.NotImplementedException();
    }

}
