using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(CleanupSystemGroup))]
    public partial struct CleanupProjectileSpawnEventSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (EnabledRefRW<ProjectileSpawnEvent> spawnEvent 
                     in SystemAPI.Query<EnabledRefRW<ProjectileSpawnEvent>>())
            {
                spawnEvent.ValueRW = false;
            }
        }
    }
}