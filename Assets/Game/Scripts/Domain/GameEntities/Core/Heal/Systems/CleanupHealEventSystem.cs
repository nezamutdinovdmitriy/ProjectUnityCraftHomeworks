using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(CleanupSystemGroup))]
    public partial struct CleanupHealEventSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new CleanupHealEventsJob().ScheduleParallel(state.Dependency);
        }
        
        [BurstCompile]
        private partial struct CleanupHealEventsJob : IJobEntity
        {
            private void Execute(DynamicBuffer<HealEvent> events) => events.Clear();
        }
    }
}