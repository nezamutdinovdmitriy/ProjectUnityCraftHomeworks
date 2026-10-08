using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Entities;
using UnityEngine;

namespace Game.Systems
{
    [UpdateInGroup(typeof(CleanupSystemGroup))]
    [RequireMatchingQueriesForUpdate]
    public partial struct CleanupAttackHitEventSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var hitEventEnabled in SystemAPI.Query<EnabledRefRW<AttackHitEvent>>())
            {
                hitEventEnabled.ValueRW = false;
            }
        }
    }
}