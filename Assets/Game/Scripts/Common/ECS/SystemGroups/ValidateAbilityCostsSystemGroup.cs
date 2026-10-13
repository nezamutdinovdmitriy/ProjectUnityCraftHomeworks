using Unity.Entities;

namespace Game.ECS.SystemGroups
{
    [UpdateInGroup(typeof(AbilitiesSystemGroup))]
    public partial class ValidateAbilityCostsSystemGroup : ComponentSystemGroup
    {
    }
}