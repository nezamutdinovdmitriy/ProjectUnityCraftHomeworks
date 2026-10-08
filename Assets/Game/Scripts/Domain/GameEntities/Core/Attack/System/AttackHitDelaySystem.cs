using Game.Components;
using Game.UseCases;
using Unity.Entities;

namespace Game.Systems
{
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