using Game.Components;
using Game.UseCases;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [RequireMatchingQueriesForUpdate]
    public partial struct EmptyManaRestoreSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new StartManaRestoreJob().ScheduleParallel(state.Dependency);
        }
        
        [BurstCompile]
        [WithDisabled(typeof(ManaRestoreDelay))]
        private partial struct StartManaRestoreJob : IJobEntity
        {
            private void Execute(
                ref ManaRestoreDelay delay,
                EnabledRefRW<ManaRestoreDelay> delayEnabled,
                in Mana mana)
            {
                if (mana.IsEmpty() == false)
                    return;

                delay.Time = delay.Delay;
                delayEnabled.ValueRW = true;
            }
        }
    }
}