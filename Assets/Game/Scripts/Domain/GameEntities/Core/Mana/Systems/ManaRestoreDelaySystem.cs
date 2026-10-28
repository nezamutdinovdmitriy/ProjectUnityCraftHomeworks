using Game.Components;
using Game.ECS.SystemGroups;
using Game.UseCases;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(UpdateTimersSystemGroup))]
    public partial struct ManaRestoreDelaySystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new RestoreDelayJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime
            }.ScheduleParallel(state.Dependency);
        }
        
        [BurstCompile]
        [WithPresent(typeof(ManaRestoreEvent))]
        private partial struct RestoreDelayJob : IJobEntity
        {
            public float DeltaTime;
            
            private void Execute(
                ref ManaRestoreDelay delay,
                ref Mana mana,
                in MaxMana maxMana,
                EnabledRefRW<ManaRestoreDelay> delayEnabled,
                EnabledRefRW<ManaRestoreEvent> eventEnabled)
            {
                delay.Time -= DeltaTime;

                if (delay.IsExpired())
                {
                    mana.Value = maxMana.Value;

                    delayEnabled.ValueRW = false;
                    eventEnabled.ValueRW = true;
                }
            }
        }
    }
}