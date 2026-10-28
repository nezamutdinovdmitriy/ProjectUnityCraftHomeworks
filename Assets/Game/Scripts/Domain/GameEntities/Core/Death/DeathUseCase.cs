using Game.Components;
using Unity.Entities;

namespace Game.UseCases
{
    public static class DeathUseCase
    {
        public static void StartProcess(
            ref DeathCooldown cooldown, 
            EnabledRefRW<DeathCooldown> cooldownEnabled,
            EnabledRefRW<DeathEvent> deathEvent)
        {
            cooldown.Time = cooldown.Duration;
            cooldownEnabled.ValueRW = true;
            deathEvent.ValueRW = true;
        }
    }
}