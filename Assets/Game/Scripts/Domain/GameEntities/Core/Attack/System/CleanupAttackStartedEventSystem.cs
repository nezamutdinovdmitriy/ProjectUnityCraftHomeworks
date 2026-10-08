using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Entities;
using UnityEngine;

namespace Game.Systems
{
    [UpdateInGroup(typeof(CleanupSystemGroup))]
    [RequireMatchingQueriesForUpdate]
    public partial struct CleanupAttackStartedEventSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var startedEventEnabled in SystemAPI.Query<EnabledRefRW<AttackStartedEvent>>())
            {
                startedEventEnabled.ValueRW = false;
            }
        }
    }
}