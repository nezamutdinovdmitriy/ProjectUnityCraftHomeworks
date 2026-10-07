using Game.Components;
using Game.UseCases;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Game.Systems
{
    public partial struct TargetDetectionSystem : ISystem
    {
        private ComponentLookup<Team> _teamLookup;
        private ComponentLookup<Health> _healthLookup;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsWorldSingleton>();

            _teamLookup = SystemAPI.GetComponentLookup<Team>();
            _healthLookup = SystemAPI.GetComponentLookup<Health>();
        }

        public void OnUpdate(ref SystemState state)
        {
            _healthLookup.Update(ref state);
            _teamLookup.Update(ref state);

            CollisionWorld collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;

            foreach (var (
                         targetEntityRW,
                         defaultTargetRO,
                         selfTeamRO,
                         detectionRadiusRO,
                         selfHealthRO,
                         cooldownEnabledRW,
                         selfTransformRO,
                         selfEntity)
                     in SystemAPI.Query<
                             RefRW<TargetEntity>,
                             RefRO<DefaultTargetEntity>,
                             RefRO<Team>,
                             RefRO<TargetDetectionRadius>,
                             RefRO<Health>,
                             EnabledRefRW<TargetDetectionCooldown>,
                             RefRO<LocalTransform>>()
                         .WithAll<AIControlled>()
                         .WithDisabled<TargetDetectionCooldown>()
                         .WithEntityAccess())
            {
                if (selfHealthRO.ValueRO.IsDead())
                {
                    targetEntityRW.ValueRW.Value = Entity.Null;
                    continue;
                }

                float radius = detectionRadiusRO.ValueRO.Value;
                float3 center = selfTransformRO.ValueRO.Position + new float3(0f, 1f, 0f);
                
                NativeList<DistanceHit> hits = new NativeList<DistanceHit>(Allocator.Temp);

                collisionWorld.OverlapSphere(center, radius, ref hits, CollisionFilter.Default);

                Entity nearestEnemy = Entity.Null;
                float nearestDistance = float.MaxValue;

                foreach (DistanceHit hit in hits)
                {
                    Entity candidate = hit.Entity;

                    if (candidate == selfEntity
                        || _teamLookup.TryGetRefRO(candidate, out var candidateTeam) == false
                        || _healthLookup.TryGetRefRO(candidate, out var candidateHealth) == false
                        || candidateHealth.ValueRO.IsDead())
                        continue;

                    if (selfTeamRO.ValueRO.IsEnemy(candidateTeam.ValueRO) == false)
                        continue;

                    if (hit.Distance < nearestDistance)
                    {
                        nearestDistance = hit.Distance;
                        nearestEnemy = candidate;
                    }
                }

                if (nearestEnemy == Entity.Null)
                {
                    Entity defaultTarget = defaultTargetRO.ValueRO.Value;

                    if (defaultTarget != Entity.Null
                        && _healthLookup.TryGetRefRO(defaultTarget, out var defaultTargetHealthRO)
                        && defaultTargetHealthRO.ValueRO.IsAlive())
                        nearestEnemy = defaultTarget;
                }
                
                targetEntityRW.ValueRW.Value = nearestEnemy;
                cooldownEnabledRW.ValueRW = true;
                hits.Dispose();
            }
        }
    }
}