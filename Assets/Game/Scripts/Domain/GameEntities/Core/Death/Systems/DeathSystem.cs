using Game.Components;
using Unity.Entities;

namespace Game.Systems
{
    public partial struct DeathSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<DeathEvent>();
            state.RequireForUpdate<Health>();
        }

        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            EntityCommandBuffer ecb = SystemAPI
                .GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            HandleDeath(ref state);
            TickDeathCooldown(ref state, deltaTime, ecb);
        }

        private void TickDeathCooldown(ref SystemState state, float deltaTime, EntityCommandBuffer ecb)
        {
            foreach ((RefRW<DeathCooldown> cooldown, Entity entity)
                     in SystemAPI.Query<RefRW<DeathCooldown>>().WithEntityAccess())
            {
                cooldown.ValueRW.Time -= deltaTime;

                if (cooldown.ValueRO.Time <= 0)
                    ecb.DestroyEntity(entity);
            }
        }

        private void HandleDeath(ref SystemState state)
        {
            foreach ((RefRW<DeathCooldown> cooldown,
                         EnabledRefRW<DeathCooldown> cooldownEnabled,
                         EnabledRefRW<DeathEvent> deathEvent,
                         RefRO<Health> health)
                     in SystemAPI.Query<
                             RefRW<DeathCooldown>,
                             EnabledRefRW<DeathCooldown>,
                             EnabledRefRW<DeathEvent>,
                             RefRO<Health>>()
                         .WithDisabled<DeathCooldown>()
                         .WithPresent<DeathEvent>())
            {
                if (health.ValueRO.IsAlive())
                    continue;

                cooldown.ValueRW.Time = cooldown.ValueRO.Duration;
                cooldownEnabled.ValueRW = true;
                deathEvent.ValueRW = true;
            }
        }
    }
}