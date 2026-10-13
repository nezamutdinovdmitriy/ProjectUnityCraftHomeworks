using Game.Components;
using Game.ECS.SystemGroups;
using Game.UseCases;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(UpdateTimersSystemGroup))]
    [BurstCompile]
    public partial struct DetectionCooldownSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (cooldownRW,
                         cooldownEnabled) 
                     in SystemAPI.Query<
                         RefRW<TargetDetectionCooldown>,
                         EnabledRefRW<TargetDetectionCooldown>>())
            {
                float deltaTime = SystemAPI.Time.DeltaTime;

                cooldownRW.ValueRW.Time -= deltaTime;

                if (cooldownRW.ValueRO.IsExpired())
                {
                    cooldownRW.ValueRW.Time = cooldownRW.ValueRO.Interval;
                    cooldownEnabled.ValueRW = false;
                }
            }
        }
    }
}