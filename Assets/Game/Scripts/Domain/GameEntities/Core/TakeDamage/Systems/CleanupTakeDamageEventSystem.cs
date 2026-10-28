using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(CleanupSystemGroup))]
    public partial struct CleanupTakeDamageEventSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new ClearDamageEventsJob()
                .ScheduleParallel(state.Dependency);
        }
        
        [BurstCompile]
        private partial struct ClearDamageEventsJob : IJobEntity
        {
            private void Execute(DynamicBuffer<TakeDamageEvent> events) 
                => events.Clear();
        }
    }
}