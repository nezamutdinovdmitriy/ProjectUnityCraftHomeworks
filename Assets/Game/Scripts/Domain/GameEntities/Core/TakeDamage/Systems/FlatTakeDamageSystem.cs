using Game.Components;
using Game.Scripts.Domain.GameEntities.Core.TakeDamage;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    public partial struct FlatTakeDamageSystem : ISystem
    {
        public void OnCreate(ref SystemState state) 
            => state.RequireForUpdate<TakeDamageRequest>();

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
                TakeDamageUseCase.ApplyDamage(events, requests, ref health.ValueRW);
            }
        }
    }
}