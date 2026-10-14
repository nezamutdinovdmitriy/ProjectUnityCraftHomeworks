using Game.Components;
using Unity.Entities;

namespace Game.ECS.SystemGroups
{
    [UpdateInGroup(typeof(AbilitiesSystemGroup))]
    [UpdateAfter(typeof(ExecuteAbilitiesSystemGroup))]
    public partial class ConsumeAbilityResourcesSystemGroup : ComponentSystemGroup 
    {
    }
}