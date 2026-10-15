using Game.ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(AbilitiesSystemGroup))]
    [BurstCompile]
    public partial struct ArrowShotSystem : ISystem
    {
        
    }
}