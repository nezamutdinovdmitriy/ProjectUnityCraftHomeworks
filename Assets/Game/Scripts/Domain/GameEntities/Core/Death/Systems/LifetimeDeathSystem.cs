using Game.Components;
using Game.UseCases;
using Unity.Entities;

namespace Game.Systems
{
    [RequireMatchingQueriesForUpdate]
    public partial struct LifetimeDeathSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach ((RefRW<Lifetime> lifetime,
                         RefRW<DeathCooldown> deathCooldown,
                         EnabledRefRW<DeathCooldown> cooldownEnabled,
                         EnabledRefRW<DeathEvent> deathEvent)
                     in SystemAPI.Query<
                             RefRW<Lifetime>,
                             RefRW<DeathCooldown>,
                             EnabledRefRW<DeathCooldown>,
                             EnabledRefRW<DeathEvent>>()
                         .WithDisabled<DeathCooldown>()
                         .WithPresent<DeathEvent>())
            {
                if (lifetime.ValueRO.IsExpired() == false)
                {
                    lifetime.ValueRW.Value -= deltaTime;
                    continue;
                }

                DeathUseCase.StartProcess(deathCooldown, cooldownEnabled, deathEvent);
            }
        }
    }
}