using Game.Components;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    public partial struct FlatTakeDamageSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<TakeDamageRequest>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach ((DynamicBuffer<TakeDamageEvent> events,
                         DynamicBuffer<TakeDamageRequest> requests,
                         RefRW<Health> health)
                     in SystemAPI.Query<
                         DynamicBuffer<TakeDamageEvent>,
                         DynamicBuffer<TakeDamageRequest>,
                         RefRW<Health>>()
                         .WithNone<Armor>())
            {
                for (int i = 0; i < requests.Length && health.ValueRO.IsAlive(); i++)
                {
                    TakeDamageRequest request = requests[i];

                    health.ValueRW.ReduceHealth(request.Damage);
                    
                    events.Add(new TakeDamageEvent
                    {
                        Damage = request.Damage
                    });
                }
                
                requests.Clear();
            }
        }
    }
}