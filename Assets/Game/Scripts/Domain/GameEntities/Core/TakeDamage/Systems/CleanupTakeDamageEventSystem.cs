using Game.Components;
using Game.ECS.SystemGroups;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [UpdateInGroup(typeof(CleanupSystemGroup))]
    public partial struct CleanupTakeDamageEventSystem : ISystem
    {
        [BurstCompile]
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