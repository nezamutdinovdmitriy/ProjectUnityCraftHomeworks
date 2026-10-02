using Game.Components;
using Game.Scripts.Domain.GameEntities.Core.TakeDamage;
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

                TakeDamageUseCase.ApplyDamage(events, requests, ref health.ValueRW, multiplier);
            }
        }
    }
}