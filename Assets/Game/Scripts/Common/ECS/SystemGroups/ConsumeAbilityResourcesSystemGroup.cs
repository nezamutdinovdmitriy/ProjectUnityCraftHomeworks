using Game.Components;
using Unity.Entities;

namespace Game.ECS.SystemGroups
{
    [UpdateInGroup(typeof(AbilitiesSystemGroup))]
    [UpdateAfter(typeof(ExecuteAbilitiesSystemGroup))]
    public partial class ConsumeAbilityResourcesSystemGroup : ComponentSystemGroup 
    {
        protected override void OnUpdate()
        {
            base.OnUpdate();

            foreach (var abilityCostPendingEnabled in SystemAPI.Query<EnabledRefRW<AbilityCostPending>>())
                abilityCostPendingEnabled.ValueRW = false;
        }
    }
}