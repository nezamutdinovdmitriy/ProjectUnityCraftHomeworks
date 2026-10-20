using Unity.Entities;

namespace Game.ECS.SystemGroups
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(UpdateTimersSystemGroup))]
    public partial class GenerateRequestsSystemGroup : ComponentSystemGroup
    {
        
    }
}