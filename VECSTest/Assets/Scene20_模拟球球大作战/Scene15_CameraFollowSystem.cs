using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

namespace Scene15
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(Scene15_PlayerMoveSystem))]
    public class Scene15_CameraFollowSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
           
        }


    }
}
