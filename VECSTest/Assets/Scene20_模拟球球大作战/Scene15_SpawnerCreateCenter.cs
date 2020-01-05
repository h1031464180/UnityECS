using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Scene15
{

    public class Scene15_SpawnerCreateCenter : MonoBehaviour
    {
        public GameObject playerPrefab;
        public GameObject cameraPrefab;
        private EntityManager entityManager;
        void Start()
        {
            this.entityManager = World.Active.EntityManager;
            Entity tmpEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(playerPrefab, World.Active);
            var entity = this.entityManager.Instantiate(tmpEntity);
            entityManager.AddComponentData<SpawnerData>(entity, new SpawnerData
            {
                energyNum = 0,
                colliderRadius = 1
            });
            entityManager.AddComponentData<PlayerInput>(entity, new PlayerInput
            {
                Vector = new float3()
            });
            entityManager.AddComponentData<PlayerData>(entity, new PlayerData
            {
                moveSpeed = 5
            });
            entityManager.SetComponentData<Translation>(entity, new Translation
            {
                Value = new float3()
            });

            Entity cameraEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(cameraPrefab, World.Active);
            var tmpCamera = this.entityManager.Instantiate(cameraEntity);

            entityManager.SetComponentData<Translation>(tmpCamera, new Translation
            {
                Value = new float3()
            });
        }

    }
}