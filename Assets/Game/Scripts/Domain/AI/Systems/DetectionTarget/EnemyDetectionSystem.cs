using Game.Components;
using Game.Predicates;
using Game.UseCases;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Game.Systems
{
    public partial struct EnemyDetectionSystem : ISystem
    {
        private ComponentLookup<Team> _teamLookup;
        private ComponentLookup<Health> _healthLookup;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsWorldSingleton>();

            _teamLookup = SystemAPI.GetComponentLookup<Team>(true);
            _healthLookup = SystemAPI.GetComponentLookup<Health>(true);
        }

        public void OnUpdate(ref SystemState state)
        {
            _healthLookup.Update(ref state);
            _teamLookup.Update(ref state);

            CollisionWorld collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;

            NativeList<DistanceHit> hits = new NativeList<DistanceHit>(Allocator.Temp);
            
            foreach (var (
                         targetRW,
                         defaultTargetRO,
                         selfTeamRO,
                         radiusRO,
                         selfHealthRO,
                         cooldownEnabled,
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
                         .WithAbsent<WoundedAllyTargeting>()
                         .WithDisabled<TargetDetectionCooldown>()
                         .WithEntityAccess())
            {
                if (selfHealthRO.ValueRO.IsAlive() == false)
                {
                    targetRW.ValueRW.Value = Entity.Null;
                    continue;
                }
                
                hits.Clear();
                
                float radius = radiusRO.ValueRO.Value;
                float3 center = selfTransformRO.ValueRO.Position + new float3(0f, 1f, 0f);

                if (radius > 0f)
                    collisionWorld.OverlapSphere(center, radius, ref hits, CollisionFilter.Default);
                
                IsEnemyPredicate predicate = new IsEnemyPredicate(
                    selfEntity,
                    selfTeamRO.ValueRO.Value,
                    _teamLookup,
                    _healthLookup);

                Entity selectedTarget = AIUseCase.FindClosestTarget(hits, predicate);

                if (selectedTarget == Entity.Null)
                {
                    Entity defaultTarget = defaultTargetRO.ValueRO.Value;
                    
                    if (predicate.Invoke(defaultTarget))
                        selectedTarget = defaultTarget;
                }

                targetRW.ValueRW.Value = selectedTarget;
                cooldownEnabled.ValueRW = true;
            }

            hits.Dispose();
        }
    }
}