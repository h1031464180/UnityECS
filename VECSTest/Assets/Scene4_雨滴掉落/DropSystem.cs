using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class DropSystem : ComponentSystem
{
    public float minHeight = -100;                              // 下落最低值
    protected override void OnUpdate()
    {
        // 通过ECS提供的筛方式遍历场景中包含该数据组件的实体
        Entities.ForEach((ref Translation translation, ref DropData dropData) =>
        {

            if (dropData.delay > 0)
            {
                dropData.delay -= Time.deltaTime;
            }
            else
            {
                if (translation.Value.y < minHeight)
                {
                    translation.Value.y = 0;
                    dropData.velocity = Random.Range(1, 10);
                    dropData.delay = Random.Range(1,10);
                }
                else
                {
                    translation.Value.y -= dropData.velocity * Time.deltaTime;
                }
            }
        });
    }
}
