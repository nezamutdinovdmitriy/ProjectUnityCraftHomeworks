using Unity.Entities;
using Unity.Transforms;

namespace Game.ECS.SystemGroups
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(GenerateRequestsSystemGroup))]
    [UpdateAfter(typeof(TransformSystemGroup))]
    public partial class AbilitiesSystemGroup : ComponentSystemGroup
    {
        
    }
}