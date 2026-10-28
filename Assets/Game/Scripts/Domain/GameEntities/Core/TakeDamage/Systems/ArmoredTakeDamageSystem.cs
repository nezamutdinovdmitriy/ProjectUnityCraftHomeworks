using Game.Components;
using Game.UseCases;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Game.Systems
{
    [RequireMatchingQueriesForUpdate]
    public partial struct ArmoredTakeDamageSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new ApplyArmoredDamageJob()
                .ScheduleParallel(state.Dependency);
        }
        
        [BurstCompile]
        private partial struct ApplyArmoredDamageJob : IJobEntity
        {
            private void Execute(
                DynamicBuffer<TakeDamageRequest> requests,
                DynamicBuffer<TakeDamageEvent> events,
                ref Health health,
                in Armor armor)
            {
                float multiplier = 1f - math.saturate(armor.Value);

                for (int i = 0; i < requests.Length && health.IsAlive(); i++)
                {
                    float appliedDamage = TakeDamageUseCase.Apply(
                        ref health,
                        requests[i].Damage,
                        multiplier);

                    events.Add(new TakeDamageEvent
                    {
                        Damage = appliedDamage
                    });
                }
                
                requests.Clear();
            }
        }
    }
}