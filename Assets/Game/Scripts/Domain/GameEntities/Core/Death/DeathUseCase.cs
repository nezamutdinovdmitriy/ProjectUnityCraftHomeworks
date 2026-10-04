using Game.Components;
using Unity.Entities;

namespace Game.Scripts.Domain.GameEntities.Core.Death
{
    public static class DeathUseCase
    {
        public static void StartProcess(
            RefRW<DeathCooldown> cooldown, 
            EnabledRefRW<DeathCooldown> cooldownEnabled,
            EnabledRefRW<DeathEvent> deathEvent)
        {
            cooldown.ValueRW.Time = cooldown.ValueRO.Duration;
            cooldownEnabled.ValueRW = true;
            deathEvent.ValueRW = true;
        }
    }
}