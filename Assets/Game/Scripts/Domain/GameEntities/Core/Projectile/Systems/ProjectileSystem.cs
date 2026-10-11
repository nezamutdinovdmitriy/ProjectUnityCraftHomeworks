using Game.Components;
using Game.UseCases;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Systems
{
    public partial struct ProjectileSystem : ISystem
    {
        private ComponentLookup<Health> _healthLookup;
        private BufferLookup<TakeDamageRequest> _takeDamageRequestLookup;
        private ComponentLookup<TargetHitPoint> _targetHitPointLookup;
        private ComponentLookup<LocalToWorld> _worldTransformLookup;

        public void OnCreate(ref SystemState state)
        {
            _healthLookup = SystemAPI.GetComponentLookup<Health>(true);
            _takeDamageRequestLookup = SystemAPI.GetBufferLookup<TakeDamageRequest>();
            _targetHitPointLookup = SystemAPI.GetComponentLookup<TargetHitPoint>(true);
            _worldTransformLookup = SystemAPI.GetComponentLookup<LocalToWorld>(true);
        }

        public void OnUpdate(ref SystemState state)
        {
            _worldTransformLookup.Update(ref state);
            _targetHitPointLookup.Update(ref state);
            _takeDamageRequestLookup.Update(ref state);
            _healthLookup.Update(ref state);

            foreach (var (
                         damage,
                         target,
                         selfTransform,
                         movementRequest,
                         rotationRequest,
                         movementRequestEnabled,
                         rotationRequestEnabled,
                         self)
                     in SystemAPI.Query<
                             RefRO<Damage>,
                             RefRO<TargetEntity>,
                             RefRO<LocalTransform>,
                             RefRW<MovementRequest>,
                             RefRW<RotationRequest>,
                             EnabledRefRW<MovementRequest>,
                             EnabledRefRW<RotationRequest>>()
                         .WithAll<Projectile>()
                         .WithDisabled<MovementRequest, RotationRequest>()
                         .WithEntityAccess())
            {
                float3 direction = movementRequest.ValueRO.Direction;
                Entity targetEntity = target.ValueRO.Value;

                if (targetEntity != Entity.Null
                    && _healthLookup.TryGetComponent(targetEntity, out Health targetHealth)
                    && targetHealth.IsAlive()
                    && _targetHitPointLookup.TryGetComponent(targetEntity, out TargetHitPoint hitPoint)
                    && _worldTransformLookup.TryGetComponent(hitPoint.Value, out LocalToWorld hitPointTransform))
                {
                    float3 delta = hitPointTransform.Position - selfTransform.ValueRO.Position;
                    
                    float distanceSq = math.lengthsq(delta);
                    float stoppingDistance = 0.25f;

                    if (distanceSq <= stoppingDistance * stoppingDistance
                        && _takeDamageRequestLookup.TryGetBuffer(targetEntity, out var buffer))
                    {
                        buffer.Add(new TakeDamageRequest
                        {
                            Damage = damage.ValueRO.Value
                        });

                        SystemAPI.GetComponentRW<DeathCooldown>(self).ValueRW.Time = 0f;
                        SystemAPI.SetComponentEnabled<DeathCooldown>(self, true);
                        SystemAPI.SetComponentEnabled<DeathEvent>(self, true);
                        
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
    }
}