using Game.Components;
using Game.Scripts.Common.ECS.SystemGroups;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(CleanupSystemGroup))]
    public partial struct CleanupTakeDamageEventSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (DynamicBuffer<TakeDamageEvent> events 
                     in SystemAPI.Query<DynamicBuffer<TakeDamageEvent>>())
            {
                if(events.IsEmpty)
                    continue;
                
                events.Clear();
            }
        }
    }
}