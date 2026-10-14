using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(ValidateAbilityCostsSystemGroup))]
    [BurstCompile]
    public partial struct ValidateAbilityManaSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (cost, mana, requestEnabled) 
                     in SystemAPI.Query<RefRO<ManaCost>, RefRO<Mana>, EnabledRefRW<AttackStartedRequest>>()
                         .WithDisabled<AttackInProcess>())
            {
                if (cost.ValueRO.Value < 0 || mana.ValueRO.Value < cost.ValueRO.Value)
                    requestEnabled.ValueRW = false;
            }
        }
    }
}