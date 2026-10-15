// using Game.Components;
// using Game.ECS.SystemGroups;
// using Game.UseCases;
// using Unity.Entities;
// using Unity.Transforms;
// using Unity.Mathematics;
//
// namespace Game.Systems
// {
//     [UpdateInGroup(typeof(AbilitiesSystemGroup))]
//     public partial struct RangeAttackSystem : ISystem
//     {
//         private ComponentLookup<Health> _healthLookup;
//         private ComponentLookup<AttackCooldown> _attackCooldownLookup;
//         private ComponentLookup<ProjectilePrefab> _projectilePrefabLookup;
//         private ComponentLookup<ProjectileSpawnPoint> _projectileSpawnPointLookup;
//         private ComponentLookup<LocalToWorld> _localToWorldLookup;
//         private ComponentLookup<LocalTransform> _localTransformLookup;
//
//         public void OnCreate(ref SystemState state)
//         {
//             state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
//
//             _healthLookup = SystemAPI.GetComponentLookup<Health>(true);
//             _attackCooldownLookup = SystemAPI.GetComponentLookup<AttackCooldown>();
//             _projectilePrefabLookup = SystemAPI.GetComponentLookup<ProjectilePrefab>(true);
//             _projectileSpawnPointLookup = SystemAPI.GetComponentLookup<ProjectileSpawnPoint>(true);
//             _localToWorldLookup = SystemAPI.GetComponentLookup<LocalToWorld>(true);
//             _localTransformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);
//         }
//
//         public void OnUpdate(ref SystemState state)
//         {
//             state.EntityManager.CompleteDependencyBeforeRO<LocalToWorld>();
//
//             _localTransformLookup.Update(ref state);
//             _localToWorldLookup.Update(ref state);
//             _projectileSpawnPointLookup.Update(ref state);
//             _attackCooldownLookup.Update(ref state);
//             _projectilePrefabLookup.Update(ref state);
//             _healthLookup.Update(ref state);
//
//             EntityCommandBuffer ecb = SystemAPI
//                 .GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
//                 .CreateCommandBuffer(state.WorldUnmanaged);
//
//             foreach (var (
//                          attackStartedRequest,
//                          attackHitDelay,
//                          attackStartedRequestEnabled,
//                          attackCooldownEnabled,
//                          attackHitEventEnabled,
//                          attackStartedEventEnabled,
//                          attackInProcessEnabled,
//                          self)
//                      in SystemAPI.Query<
//                              RefRW<AttackStartedRequest>,
//                              RefRW<AttackHitDelay>,
//                              EnabledRefRW<AttackStartedRequest>,
//                              EnabledRefRW<AttackCooldown>,
//                              EnabledRefRW<AttackHitEvent>,
//                              EnabledRefRW<AttackStartedEvent>,
//                              EnabledRefRW<AttackInProcess>>()
//                          .WithAll<Archer>()
//                          .WithPresent<AttackInProcess, AttackHitDelay, AttackCooldown>()
//                          .WithPresent<AttackStartedEvent, AttackHitEvent>()
//                          .WithEntityAccess())
//             {
//                 if (attackInProcessEnabled.ValueRO == false)
//                 {
//                     attackInProcessEnabled.ValueRW = true;
//                     attackStartedEventEnabled.ValueRW = true;
//
//                     attackHitDelay.ValueRW.Reset();
//                     SystemAPI.SetComponentEnabled<AttackHitDelay>(self, true);
//                 }
//
//                 if (attackHitDelay.ValueRO.IsExpired() == false)
//                     continue;
//
//                 attackInProcessEnabled.ValueRW = false;
//                 attackStartedRequestEnabled.ValueRW = false;
//
//                 _attackCooldownLookup.GetRefRW(self).ValueRW.Reset();
//                 attackCooldownEnabled.ValueRW = true;
//
//                 Entity target = attackStartedRequest.ValueRO.Target;
//
//                 if (_healthLookup.TryGetRefRO(target, out RefRO<Health> targetHealth)
//                     && _healthLookup.TryGetRefRO(self, out RefRO<Health> selfHealth)
//                     && targetHealth.ValueRO.IsAlive()
//                     && selfHealth.ValueRO.IsAlive())
//                 {
//                     RefRO<ProjectilePrefab> projectilePrefab = _projectilePrefabLookup.GetRefRO(self);
//                     RefRO<ProjectileSpawnPoint> projectileSpawnPoint = _projectileSpawnPointLookup.GetRefRO(self);
//
//                     if (SpawnProjectile(
//                             ref ecb,
//                             projectilePrefab.ValueRO,
//                             projectileSpawnPoint.ValueRO,
//                             target))
//                     {
//                         attackHitEventEnabled.ValueRW = true;
//                     }
//                 }
//             }
//         }
//
//         private bool SpawnProjectile(
//             ref EntityCommandBuffer ecb,
//             in ProjectilePrefab projectilePrefab,
//             in ProjectileSpawnPoint spawnPoint,
//             in Entity target)
//         {
//             Entity prefab = projectilePrefab.Value;
//
//             if (prefab == Entity.Null
//                 || _localToWorldLookup.TryGetComponent(spawnPoint.Value, out LocalToWorld spawnPosition) == false
//                 || _localToWorldLookup.TryGetComponent(target, out LocalToWorld targetPosition) == false
//                 || _localTransformLookup.TryGetComponent(prefab, out LocalTransform projectileTransform) == false)
//                 return false;
//
//             float3 position = spawnPosition.Position;
//             float3 direction = targetPosition.Position + new float3(0, 0.5f, 0) - position;
//             
//             direction = math.normalizesafe(
//                 direction,
//                 math.normalizesafe(spawnPosition.Forward, new float3(0f, 0f, 1f)));
//             
//             quaternion modelCorrection = projectileTransform.Rotation;
//             
//             quaternion aimRotation = quaternion.LookRotationSafe(direction, math.up());
//             
//             projectileTransform.Position = position;
//             projectileTransform.Rotation = math.mul(aimRotation, modelCorrection);
//
//             Entity projectile = ecb.Instantiate(prefab);
//
//             ecb.SetComponent(projectile, projectileTransform);
//
//             ecb.SetComponent(projectile, new TargetEntity
//             {
//                 Value = target
//             });
//             
//             ecb.SetComponent(projectile, new MovementRequest
//             {
//                 Direction = direction
//             });
//
//             return true;
//         }
//     }
// }