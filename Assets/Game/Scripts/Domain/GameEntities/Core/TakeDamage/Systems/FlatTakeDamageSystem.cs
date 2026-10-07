using Game.Components;
using Game.UseCases;
using Game.Scripts.Domain.GameEntities.Core.TakeDamage;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Game.Systems
{
    [RequireMatchingQueriesForUpdate]
    public partial struct FlatTakeDamageSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach ((DynamicBuffer<TakeDamageEvent> events,
                         DynamicBuffer<TakeDamageRequest> requests,
                         RefRW<Health> health)
                     in SystemAPI.Query<
                         DynamicBuffer<TakeDamageEvent>,
                         DynamicBuffer<TakeDamageRequest>,
                         RefRW<Health>>()
                         .WithNone<Armor>())
            {
                for (int i = 0; i < requests.Length && health.ValueRO.IsAlive(); i++)
                {
                    float requestedDamage = requests[i].Damage;
                    
                    float appliedDamage = TakeDamageUseCase.Apply(ref health.ValueRW, requestedDamage);

                    events.Add(new TakeDamageEvent
                    {
                        Damage = appliedDamage
                    });
                }
                
                requests.Clear();
            }
        }
    }
}