using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
namespace Scene15
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class Scene15_PlayerInputSystem : ComponentSystem
    {

        protected override void OnUpdate()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            Entities.ForEach((ref PlayerInput playerInput) =>
            {
                var normalized = new float3();
                if (x != 0 || y != 0)
                    normalized = math.normalize(new float3(x, 0, y));
                playerInput.Vector = normalized;
            });

        }
    }
}