using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(ConsumeAbilityResourcesSystemGroup))]
    public partial struct ConsumeAbilityAmmoSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (ammo, cost) 
                     in SystemAPI.Query<RefRW<Ammo>, RefRO<AmmoCost>>()
                         .WithAll<AbilityCostPending>())
            {
                ammo.ValueRW.Value -= cost.ValueRO.Value;
            }
        }
    }
}