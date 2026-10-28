using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(UpdateTimersSystemGroup))]
    public partial struct AbilityWindupSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            state.Dependency = new UpdateWindupJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime
            }.ScheduleParallel(state.Dependency);
        }
        
        [BurstCompile]
        private partial struct UpdateWindupJob : IJobEntity
        {
            public float DeltaTime;
            
            private void Execute(
                ref AbilityWindup windup,
                EnabledRefRW<AbilityWindup> enabled)
            {
                windup.Time -= DeltaTime;

                if (windup.Time <= 0)
                    enabled.ValueRW = false;
            }
        }
    }
}