using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(UpdateTimersSystemGroup))]
    [BurstCompile]
    public partial struct AbilityWindupSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (windup, enabled)
                     in SystemAPI.Query<
                         RefRW<AbilityWindup>,
                         EnabledRefRW<AbilityWindup>>())
            {
                windup.ValueRW.Time -= deltaTime;

                if (windup.ValueRO.Time <= 0)
                    enabled.ValueRW = false;
            }
        }
    }
}