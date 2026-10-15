using Game.ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(GenerateRequestsSystemGroup))]
    [BurstCompile]
    public partial struct AbilityTargetRequestSystem : ISystem
    {
        
    }
}