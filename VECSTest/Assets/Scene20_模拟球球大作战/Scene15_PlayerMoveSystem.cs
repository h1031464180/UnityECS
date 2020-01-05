using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

namespace Scene15
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(Scene15_PlayerInputSystem))]
    public class Scene15_PlayerMoveSystem : ComponentSystem
    {
        private EntityQuery entityQuery;
        private NativeArray<float3> playerPosArray;
        protected override void OnCreate()
        {
            this.playerPosArray = new NativeArray<float3>(1, Allocator.Persistent);
            this.entityQuery = EntityManager.CreateEntityQuery(typeof(Translation), ComponentType.ReadOnly<PlayerData>(), ComponentType.ReadOnly<PlayerInput>());
        }
        protected override void OnUpdate()
        {
            Entities.With(this.entityQuery).ForEach((ref PlayerInput playerInput, ref Translation translation, ref PlayerData playerData) =>
            {

                translation.Value += new float3(playerInput.Vector * playerData.moveSpeed * Time.deltaTime);
                this.playerPosArray[0] = translation.Value;
            });
            Entities.ForEach((ref Translation translation, ref CameraData cameraData) =>
            {
                var value = this.playerPosArray[0];
                value.y = -10;
                translation.Value = value;
            });
        }

        protected override void OnDestroy()
        {
            this.playerPosArray.Dispose();
        }
    }


}