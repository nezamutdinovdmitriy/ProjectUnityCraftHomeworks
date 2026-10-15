using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(UpdateTimersSystemGroup))]
    [BurstCompile]
    public partial struct AbilityCooldownSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            
            foreach (var (cooldown, enabled) 
                     in SystemAPI.Query<
                         RefRW<AbilityCooldown>,
                         EnabledRefRW<AbilityCooldown>>())
            {
                cooldown.ValueRW.Time -= deltaTime;

                if (cooldown.ValueRO.Time <= 0)
                    enabled.ValueRW = false;
            }
        }
    }
}