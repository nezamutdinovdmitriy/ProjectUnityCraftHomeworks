using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(ValidateAbilityCostsSystemGroup))]
    [BurstCompile]
    public partial struct ValidateAbilityAmmoSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            foreach (var (cost, ammo, requestEnabled) 
                     in SystemAPI.Query<RefRO<AmmoCost>, RefRO<Ammo>, EnabledRefRW<AttackStartedRequest>>()
                         .WithDisabled<AttackInProcess>())
            {
                if (cost.ValueRO.Value < 0 || ammo.ValueRO.Value < cost.ValueRO.Value)
                    requestEnabled.ValueRW = false;
            }
        }
    }
}