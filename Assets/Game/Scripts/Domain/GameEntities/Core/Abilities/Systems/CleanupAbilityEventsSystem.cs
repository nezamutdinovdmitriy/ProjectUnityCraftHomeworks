using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(CleanupSystemGroup))]
    [RequireMatchingQueriesForUpdate]
    public partial struct CleanupAbilityEventsSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var enabled in SystemAPI.Query<EnabledRefRW<AbilityExecutedEvent>>())
            {
                enabled.ValueRW = false;
            }
            
            foreach (var enabled in SystemAPI.Query<EnabledRefRW<AbilityStartedEvent>>())
            {
                enabled.ValueRW = false;
            }
        }
    }
}