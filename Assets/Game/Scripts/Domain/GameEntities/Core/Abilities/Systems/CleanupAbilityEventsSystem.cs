using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

namespace Game.Systems
{
    [UpdateInGroup(typeof(CleanupSystemGroup))]
    [RequireMatchingQueriesForUpdate]
    public partial struct CleanupAbilityEventsSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = JobHandle.CombineDependencies(
                new ClearExecutedEventsJob().ScheduleParallel(state.Dependency),
                new ClearStartedEventsJob().ScheduleParallel(state.Dependency));
        }
        
        [BurstCompile]
        private partial struct ClearExecutedEventsJob : IJobEntity
        {
            private void Execute(EnabledRefRW<AbilityExecutedEvent> enabled) 
                => enabled.ValueRW = false;
        }
        
        [BurstCompile]
        private partial struct ClearStartedEventsJob : IJobEntity
        {
            private void Execute(EnabledRefRW<AbilityStartedEvent> enabled) 
                => enabled.ValueRW = false;
        }
    }
}