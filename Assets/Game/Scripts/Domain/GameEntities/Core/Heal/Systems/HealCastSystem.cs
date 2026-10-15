using Game.Components;
using Game.ECS.SystemGroups;
using Game.UseCases;
using Unity.Burst;
using Unity.Entities;

namespace Game.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(AbilitiesSystemGroup))]
    public partial struct HealCastSystem : ISystem
    {
        private ComponentLookup<Health> _healthLookup;
        private BufferLookup<HealRequest> _healRequestLookup;

        public void OnCreate(ref SystemState state)
        {
            _healthLookup = SystemAPI.GetComponentLookup<Health>();
            _healRequestLookup = SystemAPI.GetBufferLookup<HealRequest>();
        }

        public void OnUpdate(ref SystemState state)
        {
            _healthLookup.Update(ref state);
            _healRequestLookup.Update(ref state);
            
            foreach (var (
                         attackStartedRequest,
                         attackHitDelay,
                         attackCooldown,
                         healAmount,
                         attackStartedRequestEnabled,
                         attackInProcessEnabled,
                         attackStartedEventEnabled,
                         self)
                     in SystemAPI.Query<
                             RefRO<AttackStartedRequest>,
                             RefRW<AttackHitDelay>,
                             RefRW<AttackCooldown>,
                             RefRO<HealAmount>,
                             EnabledRefRW<AttackStartedRequest>,
                             EnabledRefRW<AttackInProcess>,
                             EnabledRefRW<AttackStartedEvent>>()
                         .WithPresent<AttackHitDelay, AttackCooldown, AttackInProcess>()
                         .WithPresent<AttackStartedEvent, AttackHitEvent>()
                         .WithEntityAccess())
            {
                if (attackInProcessEnabled.ValueRO == false)
                {
                    attackInProcessEnabled.ValueRW = true;
                    attackStartedEventEnabled.ValueRW = true;

                    attackHitDelay.ValueRW.Reset();
                    SystemAPI.SetComponentEnabled<AttackHitDelay>(self, true);
                }
                
                if (attackHitDelay.ValueRO.IsExpired() == false)
                    continue;
                
                attackInProcessEnabled.ValueRW = false;
                attackStartedRequestEnabled.ValueRW = false;
                
                attackCooldown.ValueRW.Reset();
                SystemAPI.SetComponentEnabled<AttackCooldown>(self, true);

                Entity target = attackStartedRequest.ValueRO.Target;
                
                if (_healthLookup.TryGetRefRO(target, out RefRO<Health> targetHealth)
                    && _healthLookup.TryGetRefRO(self, out RefRO<Health> selfHealth)
                    && targetHealth.ValueRO.IsAlive()
                    && selfHealth.ValueRO.IsAlive()
                    && _healRequestLookup.TryGetBuffer(target, out var buffer))
                {
                    buffer.Add(new HealRequest
                    {
                        Value = healAmount.ValueRO.Value
                    });
                    
                    SystemAPI.SetComponentEnabled<AttackHitEvent>(self, true);
                }
            }
        }
    }
}