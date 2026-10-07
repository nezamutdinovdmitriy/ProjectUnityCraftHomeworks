using Game.Components;
using Game.UseCases;
using Unity.Burst;
using Unity.Entities;

namespace Game.Scripts
{
    [BurstCompile]
    public partial struct AttackCooldownSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (
                         cooldown,
                         cooldownEnabled)
                     in SystemAPI.Query<
                         RefRW<AttackCooldown>,
                         EnabledRefRW<AttackCooldown>>())
            {
                cooldown.ValueRW.Time -= deltaTime;

                if (cooldown.ValueRO.IsExpired())
                {
                    cooldownEnabled.ValueRW = false;
                    cooldown.ValueRW.Time = cooldown.ValueRO.Duration;
                }
            }
        }
    }
}