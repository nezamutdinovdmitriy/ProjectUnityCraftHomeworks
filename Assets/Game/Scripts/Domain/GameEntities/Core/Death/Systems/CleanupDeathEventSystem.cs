using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(CleanupSystemGroup))]
    [RequireMatchingQueriesForUpdate]
    public partial struct CleanupDeathEventSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (EnabledRefRW<DeathEvent> deathEvent in SystemAPI.Query<EnabledRefRW<DeathEvent>>())
                deathEvent.ValueRW = false;
        }
    }
}