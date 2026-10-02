using Game.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Game.Systems
{
    public partial struct ArmoredTakeDamageSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Armor>();
            state.RequireForUpdate<TakeDamageRequest>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach ((DynamicBuffer<TakeDamageEvent> takeDamageEvents,
                         DynamicBuffer<TakeDamageRequest> takeDamageRequests,
                         RefRW<Health> health,
                         RefRO<Armor> armor)
                     in SystemAPI.Query<DynamicBuffer<TakeDamageEvent>,
                         DynamicBuffer<TakeDamageRequest>,
                         RefRW<Health>,
                         RefRO<Armor>>())
            {
                float reduction = math.saturate(armor.ValueRO.Value);

                for (int i = 0; i < takeDamageRequests.Length && health.ValueRO.IsAlive(); i++)
                {
                    float damage = takeDamageRequests[i].Damage * (1f - reduction);
                    
                    health.ValueRW.ReduceHealth(damage);

                    takeDamageEvents.Add(new TakeDamageEvent
                    {
                        Damage = damage
                    });
                }
                
                takeDamageRequests.Clear();
            }
        }
    }
}