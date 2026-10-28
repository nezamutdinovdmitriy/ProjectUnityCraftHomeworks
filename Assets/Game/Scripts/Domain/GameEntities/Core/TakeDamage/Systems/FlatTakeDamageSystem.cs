using Game.Components;
using Game.UseCases;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [RequireMatchingQueriesForUpdate]
    public partial struct FlatTakeDamageSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new ApplyDamageJob()
                .ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        [WithNone(typeof(Armor))]
        private partial struct ApplyDamageJob : IJobEntity
        {
            private void Execute(
                DynamicBuffer<TakeDamageRequest> requests,
                DynamicBuffer<TakeDamageEvent> events,
                ref Health health)
            {
                for (int i = 0; i < requests.Length && health.IsAlive(); i++)
                {
                    float appliedDamage = TakeDamageUseCase.Apply(ref health, requests[i].Damage);

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