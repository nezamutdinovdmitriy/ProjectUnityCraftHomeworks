using Game.Components;
using Game.Predicates;
using Game.UseCases;
using Unity.Burst;
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

        private EntityQuery _baseQuery;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsWorldSingleton>();
            state.RequireForUpdate<AIControlled>();

            _teamLookup = SystemAPI.GetComponentLookup<Team>(true);
            _healthLookup = SystemAPI.GetComponentLookup<Health>(true);
            
            _baseQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<Base>()
                .Build(ref state);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _healthLookup.Update(ref state);
            _teamLookup.Update(ref state);

            state.Dependency = new FindEnemyJob
            {
                PhysicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>(),
                Bases = _baseQuery.ToEntityArray(state.WorldUpdateAllocator),
                HealthLookup = _healthLookup,
                TeamLookup = _teamLookup,
            }.ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        [WithAll(typeof(AIControlled))]
        [WithAbsent(typeof(WoundedAllyTargeting))]
        [WithDisabled(typeof(TargetDetectionCooldown))]
        private partial struct FindEnemyJob : IJobEntity
        {
            [ReadOnly] public PhysicsWorldSingleton PhysicsWorld;
            [ReadOnly] public NativeArray<Entity> Bases;
            
            [ReadOnly] public ComponentLookup<Team> TeamLookup;
            [ReadOnly] public ComponentLookup<Health> HealthLookup;
            
            private void Execute(
                Entity self,
                ref TargetEntity target,
                in DefaultTargetEntity defaultTarget,
                in Team team,
                in TargetDetectionRadius radius,
                in Health health,
                in LocalTransform transform,
                EnabledRefRW<TargetDetectionCooldown> cooldownEnabled)
            {
                if (health.IsDead())
                {
                    target.Value = Entity.Null;
                    return;
                }

                var predicate = new IsEnemyPredicate(self, team.Value, TeamLookup, HealthLookup);

                float3 center = transform.Position + new float3(0, 1, 0);

                Entity selected = AIUseCase.FindClosestTarget(
                    ref PhysicsWorld,
                    center,
                    radius.Value,
                    in predicate);

                if (selected == Entity.Null && 
                    predicate.Invoke(defaultTarget.Value))
                    selected = defaultTarget.Value;

                if (selected == Entity.Null)
                    selected = FindEnemyBase(ref predicate);
                
                target.Value = selected;
                cooldownEnabled.ValueRW = true;
            }
            
            private Entity FindEnemyBase(ref IsEnemyPredicate predicate)
            {
                foreach (Entity candidate in Bases)
                {
                    if (predicate.Invoke(candidate))
                        return candidate;
                }

                return Entity.Null;
            }
        }
    }
}