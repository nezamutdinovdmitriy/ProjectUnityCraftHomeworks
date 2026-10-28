using Game.Components;
using Game.ECS.SystemGroups;
using Game.UseCases;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(UpdateTimersSystemGroup))]
    public partial struct DetectionCooldownSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new UpdateCooldownJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime
            }.ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        private partial struct UpdateCooldownJob : IJobEntity
        {
            public float DeltaTime;
            
            private void Execute(
                RefRW<TargetDetectionCooldown> cooldown,
                EnabledRefRW<TargetDetectionCooldown> enabled)
            {
                cooldown.ValueRW.Time -= DeltaTime;

                if (cooldown.ValueRO.IsExpired())
                {
                    cooldown.ValueRW.Time = cooldown.ValueRO.Interval;
                    enabled.ValueRW = false;
                }
            }
        }
    }
}