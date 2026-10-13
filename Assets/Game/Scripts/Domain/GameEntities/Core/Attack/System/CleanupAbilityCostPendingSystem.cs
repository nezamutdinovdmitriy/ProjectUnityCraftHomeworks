using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(CleanupSystemGroup))]
    public partial struct CleanupAbilityCostPendingSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var costPending
                     in SystemAPI.Query<EnabledRefRW<AbilityCostPending>>())
                costPending.ValueRW = false;
        }
    }
}