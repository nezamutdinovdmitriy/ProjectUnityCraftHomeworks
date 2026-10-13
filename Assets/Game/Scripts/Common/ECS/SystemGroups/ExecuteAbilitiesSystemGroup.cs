using Unity.Entities;

namespace Game.ECS.SystemGroups
{
    [UpdateInGroup(typeof(AbilitiesSystemGroup))]
    [UpdateAfter(typeof(ValidateAbilityCostsSystemGroup))]
    public partial class ExecuteAbilitiesSystemGroup : ComponentSystemGroup
    {
    }
}