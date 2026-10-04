using Game.Components;
using Unity.Entities;

namespace Game.Systems
{
    [RequireMatchingQueriesForUpdate]
    public partial struct LifetimeSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            EntityCommandBuffer ecb = SystemAPI
                .GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            foreach ((RefRW<Lifetime> lifetime,
                         RefRW<DeathCooldown> deathCooldown,
                         EnabledRefRW<DeathEvent> deathEvent,
                         Entity entity)
                     in SystemAPI.Query<
                             RefRW<Lifetime>,
                             RefRW<DeathCooldown>,
                             EnabledRefRW<DeathEvent>>()
                         .WithPresent<DeathEvent>()
                         .WithPresent<DeathCooldown>()
                         .WithEntityAccess())
            {
                if (lifetime.ValueRO.IsExpired() == false)
                {
                    lifetime.ValueRW.Value -= deltaTime;
                    continue;
                }
                
                ecb.DestroyEntity(entity);
            }
        }
    }
}