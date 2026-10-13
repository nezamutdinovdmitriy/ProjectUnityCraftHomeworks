using Game.Components;
using Game.ECS.SystemGroups;
using Game.UseCases;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(UpdateTimersSystemGroup))]
    [BurstCompile]
    public partial struct AttackHitDelaySystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (
                         attackHitDelay,
                         attackHitDelayEnabled) 
                     in SystemAPI.Query<
                         RefRW<AttackHitDelay>,
                         EnabledRefRW<AttackHitDelay>>())
            {
                float deltaTime = SystemAPI.Time.DeltaTime;

                attackHitDelay.ValueRW.Time -= deltaTime;

                if (attackHitDelay.ValueRO.IsExpired())
                    attackHitDelayEnabled.ValueRW = false;
            }
        }
    }
}