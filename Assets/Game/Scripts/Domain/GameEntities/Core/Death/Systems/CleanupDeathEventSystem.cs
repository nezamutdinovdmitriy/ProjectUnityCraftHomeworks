using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(CleanupSystemGroup))]
    [RequireMatchingQueriesForUpdate]
    public partial struct CleanupDeathEventSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new CleanupDeathEventsJob().ScheduleParallel(state.Dependency);
        }
        
        [BurstCompile]
        private partial struct CleanupDeathEventsJob : IJobEntity
        {
            private void Execute(EnabledRefRW<DeathEvent> enabled) 
                => enabled.ValueRW = false;
        }
    }
}