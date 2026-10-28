using Game.Components;
using Game.UseCases;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Game.Systems
{
    [BurstCompile]
    public partial struct ApplyHealSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new ApplyHealJob()
                .ScheduleParallel(state.Dependency);
        }
        
        [BurstCompile]
        private partial struct ApplyHealJob : IJobEntity
        {
            private void Execute(
                DynamicBuffer<HealRequest> requests,
                DynamicBuffer<HealEvent> events,
                ref Health health,
                in MaxHealth maxHealth)
            {
                for (int i = 0; i < requests.Length; i++)
                {
                    if(health.IsDead())
                        break;

                    float missingHealth = math.max(0, maxHealth.Value - health.Value);
                    float restored = math.min(requests[i].Value, missingHealth);

                    if (restored <= 0)
                        continue;

                    health.Value += restored;

                    events.Add(new HealEvent
                    {
                        Value = restored
                    });
                }
                
                requests.Clear();
            }
        }
    }
}