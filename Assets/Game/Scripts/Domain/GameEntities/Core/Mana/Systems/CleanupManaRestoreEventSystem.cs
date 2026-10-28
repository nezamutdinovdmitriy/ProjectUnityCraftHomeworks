using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(CleanupSystemGroup))]
    [RequireMatchingQueriesForUpdate]
    public partial struct CleanupManaRestoreEventSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new CleanupRestoreEventsJob().ScheduleParallel(state.Dependency);
        }
        
        [BurstCompile]
        private partial struct CleanupRestoreEventsJob : IJobEntity
        {
            private void Execute(EnabledRefRW<ManaRestoreEvent> enabled) 
                => enabled.ValueRW = false;
        }
    }
}