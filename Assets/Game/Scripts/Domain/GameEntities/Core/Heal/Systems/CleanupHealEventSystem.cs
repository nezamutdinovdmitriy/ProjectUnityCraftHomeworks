using Game.Components;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
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