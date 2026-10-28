using Game.Components;
using Game.UseCases;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [RequireMatchingQueriesForUpdate]
    public partial struct HealthDeathSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new StartDeathJob().ScheduleParallel(state.Dependency);
        }
        
        [BurstCompile]
        [WithDisabled(typeof(DeathCooldown))]
        [WithPresent(typeof(DeathEvent))]
        private partial struct StartDeathJob : IJobEntity
        {
            private void Execute(
                ref DeathCooldown cooldown,
                EnabledRefRW<DeathCooldown> cooldownEnabled,
                EnabledRefRW<DeathEvent> events,
                in Health health)
            {
                if (health.IsAlive())
                    return;
                
                DeathUseCase.StartProcess(ref cooldown, cooldownEnabled, events);
            }
        }
    }
}