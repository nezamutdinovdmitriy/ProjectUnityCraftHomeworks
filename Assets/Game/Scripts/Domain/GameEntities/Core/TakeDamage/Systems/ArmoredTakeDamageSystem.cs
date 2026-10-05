using Game.Components;
using Game.Components.UseCases;
using Game.Scripts.Domain.GameEntities.Core.TakeDamage;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Game.Systems
{
    [RequireMatchingQueriesForUpdate]
    public partial struct ArmoredTakeDamageSystem : ISystem
    {
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach ((DynamicBuffer<TakeDamageEvent> events,
                         DynamicBuffer<TakeDamageRequest> requests,
                         RefRW<Health> health,
                         RefRO<Armor> armor)
                     in SystemAPI.Query<DynamicBuffer<TakeDamageEvent>,
                         DynamicBuffer<TakeDamageRequest>,
                         RefRW<Health>,
                         RefRO<Armor>>())
            {
                float multiplier = 1f - math.saturate(armor.ValueRO.Value);

                for (int i = 0; i < requests.Length && health.ValueRO.IsAlive(); i++)
                {
                    float requestedDamage = requests[i].Damage;
                    
                    float appliedDamage = TakeDamageUseCase
                        .Apply(ref health.ValueRW, requestedDamage, multiplier);
                    
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