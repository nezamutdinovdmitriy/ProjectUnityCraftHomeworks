using Unity.Entities;

namespace Game.ECS.SystemGroups
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class UpdateTimersSystemGroup : ComponentSystemGroup
    {
    }
}