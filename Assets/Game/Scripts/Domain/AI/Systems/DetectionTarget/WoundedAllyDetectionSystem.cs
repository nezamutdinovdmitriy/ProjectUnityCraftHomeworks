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
    public partial struct WoundedAllyDetectionSystem : ISystem
    {
        private ComponentLookup<Team> _teamLookup;
        private ComponentLookup<Health> _healthLookup;
        private ComponentLookup<MaxHealth> _maxHealthLookup;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsWorldSingleton>();

            _teamLookup = state.GetComponentLookup<Team>(true);
            _healthLookup = state.GetComponentLookup<Health>(true);
            _maxHealthLookup = state.GetComponentLookup<MaxHealth>(true);
        }

        public void OnUpdate(ref SystemState state)
        {
            _teamLookup.Update(ref state);
            _healthLookup.Update(ref state);
            _maxHealthLookup.Update(ref state);

            CollisionWorld collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;

            NativeList<DistanceHit> hits = new NativeList<DistanceHit>(Allocator.Temp);

            foreach (var (
                         targetRW,
                         selfTeamRO,
                         radiusRO,
                         selfHealthRO,
                         cooldownEnabled,
                         selfTransformRO,
                         selfEntity)
                     in SystemAPI.Query<
                             RefRW<TargetEntity>,
                             RefRO<Team>,
                             RefRO<TargetDetectionRadius>,
                             RefRO<Health>,
                             EnabledRefRW<TargetDetectionCooldown>,
                             RefRO<LocalTransform>>()
                         .WithAll<AIControlled, WoundedAllyTargeting>()
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

                var predicate = new IsWoundedAllyPredicate(
                    selfEntity,
                    selfTeamRO.ValueRO.Value,
                    _teamLookup,
                    _healthLookup,
                    _maxHealthLookup);

                targetRW.ValueRW.Value = AIUseCase.FindClosestTarget(hits, in predicate);

                cooldownEnabled.ValueRW = true;
            }

            hits.Dispose();
        }
    }
}