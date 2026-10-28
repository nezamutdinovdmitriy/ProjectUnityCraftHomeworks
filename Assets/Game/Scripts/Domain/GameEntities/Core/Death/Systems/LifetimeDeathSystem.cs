using Game.Components;
using Game.ECS.SystemGroups;
using Game.UseCases;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(UpdateTimersSystemGroup))]
    public partial struct LifetimeDeathSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new UpdateLifeTimeJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime
            }.ScheduleParallel(state.Dependency);
        }
        
        [BurstCompile]
        [WithDisabled(typeof(DeathCooldown))]
        [WithPresent(typeof(DeathEvent))]
        private partial struct UpdateLifeTimeJob : IJobEntity
        {
            public float DeltaTime;
            
            private void Execute(
                ref Lifetime lifetime,
                ref DeathCooldown cooldown,
                EnabledRefRW<DeathCooldown> cooldownEnabled,
                EnabledRefRW<DeathEvent> eventEnabled)
            {
                if (lifetime.IsExpired() == false)
                {
                    lifetime.Value -= DeltaTime;
                    return;
                }
                
                DeathUseCase.StartProcess(ref cooldown, cooldownEnabled, eventEnabled);
            }
        }
    }
}