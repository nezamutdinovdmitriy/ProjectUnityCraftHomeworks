using Game.Components;
using Game.Scripts.Common.ECS.SystemGroups;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(CleanupSystemGroup))]
    public partial struct CleanupDeathEventSystem : ISystem
    {
        public void OnCreate(ref SystemState state) => state.RequireForUpdate<DeathEvent>();

        public void OnUpdate(ref SystemState state)
        {
            foreach (EnabledRefRW<DeathEvent> deathEvent in SystemAPI.Query<EnabledRefRW<DeathEvent>>())
                deathEvent.ValueRW = false;
        }
    }
}