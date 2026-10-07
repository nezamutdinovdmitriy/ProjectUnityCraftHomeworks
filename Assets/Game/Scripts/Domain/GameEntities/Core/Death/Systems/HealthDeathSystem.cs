using Game.Components;
using Game.UseCases;
using Unity.Entities;

namespace Game.Systems
{
    [RequireMatchingQueriesForUpdate]
    public partial struct HealthDeathSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
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

                DeathUseCase.StartProcess(cooldown, cooldownEnabled, deathEvent);
            }
        }
    }
}