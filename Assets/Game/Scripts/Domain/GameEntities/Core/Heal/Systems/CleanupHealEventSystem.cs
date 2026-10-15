using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(CleanupSystemGroup))]
    public partial struct CleanupHealEventSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (DynamicBuffer<HealEvent> events in SystemAPI.Query<DynamicBuffer<HealEvent>>())
            {
                if (events.IsEmpty)
                    continue;
                
                events.Clear();
            }
        }
    }
}