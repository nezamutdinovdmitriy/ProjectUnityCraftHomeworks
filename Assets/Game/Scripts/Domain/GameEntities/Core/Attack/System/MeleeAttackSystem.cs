// using Game.Components;
// using Game.ECS.SystemGroups;
// using Game.UseCases;
// using Unity.Entities;
//
// namespace Game.Systems
// {
//     [UpdateInGroup(typeof(AbilitiesSystemGroup))]
//     public partial struct MeleeAttackSystem : ISystem
//     {
//         private ComponentLookup<Damage> _damageLookup;
//         private BufferLookup<TakeDamageRequest> _takeDamageRequestLookup;
//         private ComponentLookup<Health> _healthLookup;
//         private ComponentLookup<AttackCooldown> _attackCooldownLookup;
//
//         public void OnCreate(ref SystemState state)
//         {
//             _damageLookup = SystemAPI.GetComponentLookup<Damage>();
//             _takeDamageRequestLookup = SystemAPI.GetBufferLookup<TakeDamageRequest>();
//             _healthLookup = SystemAPI.GetComponentLookup<Health>();
//             _attackCooldownLookup = SystemAPI.GetComponentLookup<AttackCooldown>();
//         }
//
//         public void OnUpdate(ref SystemState state)
//         {
//             _attackCooldownLookup.Update(ref state);
//             _healthLookup.Update(ref state);
//             _damageLookup.Update(ref state);
//             _takeDamageRequestLookup.Update(ref state);
//
//             foreach (var (
//                          attackStartedRequest,
//                          attackStartedRequestEnabled,
//                          attackCooldown,
//                          attackHitDelay,
//                          attackHitEvent,
//                          attackStartedEvent,
//                          attackInProcess,
//                          self)
//                      in SystemAPI.Query<
//                              RefRW<AttackStartedRequest>,
//                              EnabledRefRW<AttackStartedRequest>,
//                              EnabledRefRW<AttackCooldown>,
//                              RefRW<AttackHitDelay>,
//                              EnabledRefRW<AttackHitEvent>,
//                              EnabledRefRW<AttackStartedEvent>,
//                              EnabledRefRW<AttackInProcess>>()
//                          .WithAll<Swordman>()
//                          .WithPresent<AttackInProcess, AttackHitDelay, AttackCooldown>()
//                          .WithPresent<AttackStartedEvent, AttackHitEvent>()
//                          .WithEntityAccess())
//             {
//                 if (attackInProcess.ValueRO == false)
//                 {
//                     attackInProcess.ValueRW = true;
//                     attackStartedEvent.ValueRW = true;
//
//                     attackHitDelay.ValueRW.Reset();
//                     SystemAPI.SetComponentEnabled<AttackHitDelay>(self, true);
//                 }
//
//                 if (attackHitDelay.ValueRO.IsExpired() == false)
//                     continue;
//                 
//                 attackInProcess.ValueRW = false;
//                 attackStartedRequestEnabled.ValueRW = false;
//                 
//                 _attackCooldownLookup.GetRefRW(self).ValueRW.Reset();
//                 attackCooldown.ValueRW = true;
//                 
//                 Entity target = attackStartedRequest.ValueRO.Target;
//
//                 if (_healthLookup.TryGetRefRO(target, out RefRO<Health> targetHealth)
//                     && _healthLookup.TryGetRefRO(self, out RefRO<Health> selfHealth)
//                     && targetHealth.ValueRO.IsAlive()
//                     && selfHealth.ValueRO.IsAlive()
//                     && _takeDamageRequestLookup.TryGetBuffer(target, out var buffer))
//                 {
//                     buffer.Add(new TakeDamageRequest
//                     {
//                         Damage = _damageLookup[self].Value
//                     });
//                     
//                     attackHitEvent.ValueRW = true;
//                 }
//             }   
//         }
//     }
// }