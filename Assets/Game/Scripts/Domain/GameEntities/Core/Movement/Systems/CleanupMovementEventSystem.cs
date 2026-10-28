using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(CleanupSystemGroup))]
    public partial struct CleanupMovementEventSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new CleanupMovementEventsJob().ScheduleParallel(state.Dependency);
        }
        
        [BurstCompile]
        private partial struct CleanupMovementEventsJob : IJobEntity
        {
            private void Execute(EnabledRefRW<MovementEvent> enabled) 
                => enabled.ValueRW = false;
        }
    }
}