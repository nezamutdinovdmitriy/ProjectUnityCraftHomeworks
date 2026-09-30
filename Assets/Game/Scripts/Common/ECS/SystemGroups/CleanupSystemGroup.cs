using Unity.Entities;

namespace Game.Scripts.Common.ECS.SystemGroups
{
    [UpdateInGroup(typeof(PresentationSystemGroup), OrderLast = true)]
    public partial class CleanupSystemGroup : ComponentSystemGroup
    {
    }
}