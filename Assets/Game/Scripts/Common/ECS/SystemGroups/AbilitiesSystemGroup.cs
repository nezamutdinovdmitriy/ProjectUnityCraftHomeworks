using Unity.Entities;

namespace Game.ECS.SystemGroups
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(GenerateRequestsSystemGroup))]
    public partial class AbilitiesSystemGroup : ComponentSystemGroup
    {
        
    }
}