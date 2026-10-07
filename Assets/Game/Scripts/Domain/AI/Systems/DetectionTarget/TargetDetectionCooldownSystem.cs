using Game.Components;
using Game.UseCases;
using Unity.Entities;

namespace Game.Systems
{
    public partial struct TargetDetectionCooldownSystem : ISystem
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