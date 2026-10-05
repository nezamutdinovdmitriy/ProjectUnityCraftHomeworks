using Game.Components;
using Game.Scripts.Common.ECS.SystemGroups;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(CleanupSystemGroup))]
    [RequireMatchingQueriesForUpdate]
    public partial struct CleanupManaRestoreEventSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (EnabledRefRW<ManaRestoreEvent> restoreEvent 
                     in SystemAPI.Query<EnabledRefRW<ManaRestoreEvent>>())
            {
                restoreEvent.ValueRW = false;
            }
        }
    }
}