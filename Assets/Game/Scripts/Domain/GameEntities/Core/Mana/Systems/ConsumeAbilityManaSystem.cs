using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(ConsumeAbilityResourcesSystemGroup))]
    public partial struct ConsumeAbilityManaSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (mana, cost) 
                     in SystemAPI.Query<RefRW<Mana>, RefRO<ManaCost>>()
                         .WithAll<AbilityCostPending>())
            {
                mana.ValueRW.Value -= cost.ValueRO.Value;
            }
        }
    }
}