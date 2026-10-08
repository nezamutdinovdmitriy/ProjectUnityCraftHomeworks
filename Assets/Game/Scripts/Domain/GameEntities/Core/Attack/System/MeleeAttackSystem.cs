using Game.Components;
using Game.UseCases;
using Unity.Entities;
using UnityEngine;

namespace Game.Systems
{
    public partial struct MeleeAttackSystem : ISystem
    {
        private ComponentLookup<Damage> _damageLookup;
        private BufferLookup<TakeDamageRequest> _takeDamageRequestLookup;
        private ComponentLookup<Health> _healthLookup;


        public void OnCreate(ref SystemState state)
        {
            _damageLookup = SystemAPI.GetComponentLookup<Damage>();
            _takeDamageRequestLookup = SystemAPI.GetBufferLookup<TakeDamageRequest>();
            _healthLookup = SystemAPI.GetComponentLookup<Health>();
        }

        public void OnUpdate(ref SystemState state)
        {
            _healthLookup.Update(ref state);
            _damageLookup.Update(ref state);
            _takeDamageRequestLookup.Update(ref state);

            foreach (var (
                         attackStartedRequest,
                         attackStartedRequestEnabled,
                         attackCooldown,
                         attackHitDelay,
                         attackHitEvent,
                         attackStartedEvent,
                         attackInProcess,
                         self)
                     in SystemAPI.Query<
                             RefRW<AttackStartedRequest>,
                             EnabledRefRW<AttackStartedRequest>,
                             EnabledRefRW<AttackCooldown>,
                             RefRW<AttackHitDelay>,
                             EnabledRefRW<AttackHitEvent>,
                             EnabledRefRW<AttackStartedEvent>,
                             EnabledRefRW<AttackInProcess>>()
                         .WithAll<Swordman>()
                         .WithPresent<AttackInProcess, AttackHitDelay, AttackCooldown>()
                         .WithPresent<AttackStartedEvent, AttackHitEvent>()
                         .WithEntityAccess())
            {
                if (attackInProcess.ValueRO == false)
                {
                    attackInProcess.ValueRW = true;
                    attackStartedEvent.ValueRW = true;

                    attackHitDelay.ValueRW.Reset();
                    SystemAPI.SetComponentEnabled<AttackHitDelay>(self, true);
                }

                if (attackHitDelay.ValueRO.IsExpired() == false)
                    continue;
                
                attackStartedRequestEnabled.ValueRW = false;
                Entity target = attackStartedRequest.ValueRO.Target;

                if (_healthLookup.TryGetRefRO(target, out RefRO<Health> targetHealth)
                    && _healthLookup.TryGetRefRO(self, out RefRO<Health> selfHealth)
                    && targetHealth.ValueRO.IsAlive()
                    && selfHealth.ValueRO.IsAlive())
                {
                    attackCooldown.ValueRW = true;
                    attackHitEvent.ValueRW = true;
                    attackInProcess.ValueRW = false;
                    
                    if (_takeDamageRequestLookup.TryGetBuffer(target, out var buffer))
                    {
                        buffer.Add(new TakeDamageRequest
                        {
                            Damage = _damageLookup[self].Value
                        });
                    }
                }
            }
        }
    }
}