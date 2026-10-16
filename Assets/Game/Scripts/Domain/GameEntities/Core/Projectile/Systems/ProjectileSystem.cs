using Game.Components;
using Game.ECS.SystemGroups;
using Game.UseCases;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Systems
{
    [UpdateInGroup(typeof(GenerateRequestsSystemGroup))]
    public partial struct ProjectileSystem : ISystem
    {
        private ComponentLookup<Health> _healthLookup;
        private ComponentLookup<TargetHitPoint> _hitPointLookup;
        private ComponentLookup<LocalToWorld> _worldTransformLookup;
        private ComponentLookup<Damage> _damageLookup;
        private BufferLookup<TakeDamageRequest> _damageRequestLookup;
        private ComponentLookup<DeathCooldown> _deathCooldownLookup;
        private ComponentLookup<DeathEvent> _deathEventLookup;


        public void OnCreate(ref SystemState state)
        {
            _healthLookup = SystemAPI.GetComponentLookup<Health>(true);
            _hitPointLookup = SystemAPI.GetComponentLookup<TargetHitPoint>(true);
            _worldTransformLookup = SystemAPI.GetComponentLookup<LocalToWorld>(true);
            _damageLookup = SystemAPI.GetComponentLookup<Damage>(true);
            _damageRequestLookup = SystemAPI.GetBufferLookup<TakeDamageRequest>();
            _deathCooldownLookup = SystemAPI.GetComponentLookup<DeathCooldown>();
            _deathEventLookup = SystemAPI.GetComponentLookup<DeathEvent>();
        }

        public void OnUpdate(ref SystemState state)
        {
            state.Dependency.Complete();

            _healthLookup.Update(ref state);
            _hitPointLookup.Update(ref state);
            _worldTransformLookup.Update(ref state);
            _damageLookup.Update(ref state);
            _damageRequestLookup.Update(ref state);
            _deathCooldownLookup.Update(ref state);
            _deathEventLookup.Update(ref state);

            foreach (var (
                         target,
                         transform,
                         movementRequest,
                         rotationRequest,
                         movementRequestEnabled,
                         rotationRequestEnabled,
                         deathCooldownEnabled,
                         projectile)
                     in SystemAPI.Query<
                             RefRO<TargetEntity>,
                             RefRO<LocalTransform>,
                             RefRW<MovementRequest>,
                             RefRW<RotationRequest>,
                             EnabledRefRW<MovementRequest>,
                             EnabledRefRW<RotationRequest>,
                             EnabledRefRW<DeathCooldown>>()
                         .WithAll<Projectile>()
                         .WithDisabled<DeathCooldown>()
                         .WithPresent<MovementRequest, RotationRequest, DeathEvent>()
                         .WithEntityAccess())
            {
                float3 direction = movementRequest.ValueRO.Direction;
                Entity targetEntity = target.ValueRO.Value;

                if (TryGetTargetPosition(targetEntity, out float3 targetPosition))
                {
                    float3 delta = targetPosition - transform.ValueRO.Position;
                    const float hitRadius = 0.25f;

                    if (math.lengthsq(delta) <= hitRadius * hitRadius)
                    {
                        if (_damageLookup.TryGetRefRO(projectile, out RefRO<Damage> damage)
                            && _damageRequestLookup.TryGetBuffer(
                                targetEntity,
                                out DynamicBuffer<TakeDamageRequest> requests))
                        {
                            requests.Add(new TakeDamageRequest
                            {
                                Damage = damage.ValueRO.Value
                            });
                        }
                        
                        movementRequestEnabled.ValueRW = false;
                        rotationRequestEnabled.ValueRW = false;

                        DeathUseCase.StartProcess(
                            _deathCooldownLookup.GetRefRW(projectile),
                            deathCooldownEnabled,
                            _deathEventLookup.GetEnabledRefRW<DeathEvent>(projectile));
                        
                        continue;
                    }

                    direction = math.normalizesafe(delta, direction);
                }
                
                movementRequest.ValueRW.Direction = direction;
                movementRequestEnabled.ValueRW = true;

                rotationRequest.ValueRW.Direction = direction;
                rotationRequestEnabled.ValueRW = true;
            }
        }
        
        private bool TryGetTargetPosition(Entity target, out float3 position)
        {
            position = default;

            if (_healthLookup.TryGetRefRO(target, out var health) == false
                || health.ValueRO.IsDead())
                return false;

            if (_hitPointLookup.TryGetRefRO(target, out var hitPoint)
                && _worldTransformLookup.TryGetRefRO(hitPoint.ValueRO.Value, out var hitTransform))
            {
                position = hitTransform.ValueRO.Position;
                return true;
            }

            if (_worldTransformLookup.TryGetRefRO(target, out var targetTransform) == false)
                return false;

            position = targetTransform.ValueRO.Position;
            return true;
        }
    }
}