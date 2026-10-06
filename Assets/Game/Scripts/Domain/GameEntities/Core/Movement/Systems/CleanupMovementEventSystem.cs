using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(CleanupSystemGroup))]
    public partial struct CleanupMovementEventSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (EnabledRefRW<MovementEvent> movementEventEnabled 
                     in SystemAPI.Query<EnabledRefRW<MovementEvent>>())
            {
                movementEventEnabled.ValueRW = false;
            }
        }
    }
}