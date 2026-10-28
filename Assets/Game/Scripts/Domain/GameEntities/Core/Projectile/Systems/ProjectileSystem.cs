using Game.Components;
using Game.ECS.SystemGroups;
using Game.UseCases;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Systems
{
    [UpdateInGroup(typeof(GenerateRequestsSystemGroup))]
    public partial struct ProjectileSystem : ISystem
    {
        private const float HitRadius = 0.25f;
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new UpdateProjectilesJob
            {
                HealthLookup = SystemAPI.GetComponentLookup<Health>(true),
                HitPointsLookup = SystemAPI.GetComponentLookup<TargetHitPoint>(true),
                WorldTransformsLookup = SystemAPI.GetComponentLookup<LocalToWorld>(true),
                DamageLookup = SystemAPI.GetComponentLookup<Damage>(true),
                DamageRequestsLookup = SystemAPI.GetBufferLookup<TakeDamageRequest>()
            }.Schedule(state.Dependency);         
        }
        
        [BurstCompile]
        [WithAll(typeof(Projectile))]
        [WithDisabled(typeof(DeathCooldown))]
        [WithPresent(
            typeof(MovementRequest),
            typeof(RotationRequest),
            typeof(DeathEvent))]
        private partial struct UpdateProjectilesJob : IJobEntity
        {
            [ReadOnly] public ComponentLookup<Health> HealthLookup;
            [ReadOnly] public ComponentLookup<TargetHitPoint> HitPointsLookup;
            [ReadOnly] public ComponentLookup<LocalToWorld> WorldTransformsLookup;
            [ReadOnly] public ComponentLookup<Damage> DamageLookup;

            public BufferLookup<TakeDamageRequest> DamageRequestsLookup;

            private void Execute(
                Entity projectile,
                in TargetEntity target,
                in LocalTransform transform,
                ref MovementRequest movementRequest,
                EnabledRefRW<MovementRequest> movementRequestEnabled,
                ref RotationRequest rotationRequest,
                EnabledRefRW<RotationRequest> rotationRequestEnabled,
                ref DeathCooldown deathCooldown,
                EnabledRefRW<DeathCooldown> deathCooldownEnabled,
                EnabledRefRW<DeathEvent> deathEvent)
            {
                float3 direction = movementRequest.Direction;
                Entity targetEntity = target.Value;

                if (TryGetTargetPosition(targetEntity, out float3 targetPosition))
                {
                    float3 delta = targetPosition - transform.Position;

                    if (math.lengthsq(delta) <= HitRadius * HitRadius)
                    {
                        if (DamageLookup.TryGetRefRO(projectile, out RefRO<Damage> damage)
                            && DamageRequestsLookup.TryGetBuffer(
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
                            ref deathCooldown,
                            deathCooldownEnabled,
                            deathEvent);

                        return;
                    }

                    direction = math.normalizesafe(delta, direction);
                }

                movementRequest.Direction = direction;
                movementRequestEnabled.ValueRW = true;

                rotationRequest.Direction = direction;
                rotationRequestEnabled.ValueRW = true;
            }

            private bool TryGetTargetPosition(
                Entity target,
                out float3 position)
            {
                position = default;

                if (HealthLookup.TryGetRefRO(
                        target,
                        out RefRO<Health> health) == false
                    || health.ValueRO.IsDead())
                    return false;

                if (HitPointsLookup.TryGetRefRO(
                        target,
                        out RefRO<TargetHitPoint> hitPoint)
                    && WorldTransformsLookup.TryGetRefRO(
                        hitPoint.ValueRO.Value,
                        out RefRO<LocalToWorld> hitTransform))
                {
                    position = hitTransform.ValueRO.Position;
                    return true;
                }

                if (WorldTransformsLookup.TryGetRefRO(
                        target,
                        out RefRO<LocalToWorld> targetTransform) == false)
                    return false;

                position = targetTransform.ValueRO.Position;
                return true;
            }
        }
    }
}