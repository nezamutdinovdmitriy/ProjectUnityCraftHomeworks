using Game.Components;
using Game.UseCases;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Systems
{
    [BurstCompile]
    public partial struct ApplyHealSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (
                         requests,
                         events,
                         health,
                         maxHealth)
                     in SystemAPI.Query<
                         DynamicBuffer<HealRequest>,
                         DynamicBuffer<HealEvent>,
                         RefRW<Health>,
                         RefRO<MaxHealth>>())
            {
                foreach (HealRequest request in requests)
                {
                    if (health.ValueRO.IsDead())
                        break;

                    float restored = math.min(request.Value, math.max(0, maxHealth.ValueRO.Value - health.ValueRO.Value));

                    if (restored <= 0f)
                        continue;

                    health.ValueRW.Value += restored;
                    
                    events.Add(new HealEvent
                    {
                        Value = restored
                    });

                    Debug.Log($"Heal Event: Restored = {restored}");
                }
                
                requests.Clear();
            }
        }
    }
}