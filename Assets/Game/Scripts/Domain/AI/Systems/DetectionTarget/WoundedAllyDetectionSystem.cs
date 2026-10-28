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

            state.Dependency = new FindWoundedAllyJob
            {
                HealthLookup = _healthLookup,
                MaxHealthLookup = _maxHealthLookup,
                TeamLookup = _teamLookup,
                PhysicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>()
            }.ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        [WithAll(typeof(AIControlled), typeof(WoundedAllyTargeting))]
        [WithDisabled(typeof(TargetDetectionCooldown))]
        private partial struct FindWoundedAllyJob : IJobEntity
        {
            [ReadOnly] public ComponentLookup<Team> TeamLookup;
            [ReadOnly] public ComponentLookup<Health> HealthLookup;
            [ReadOnly] public ComponentLookup<MaxHealth> MaxHealthLookup;

            [ReadOnly] public PhysicsWorldSingleton PhysicsWorld;
            
            private void Execute(
                Entity self,
                ref TargetEntity target,
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

                var predicate = new IsWoundedAllyPredicate(
                    self,
                    team.Value,
                    TeamLookup,
                    HealthLookup,
                    MaxHealthLookup);
                
                float3 center = transform.Position + new float3(0, 1, 0);

                target.Value = AIUseCase.FindClosestTarget(
                    ref PhysicsWorld,
                    center,
                    radius.Value,
                    in predicate);

                cooldownEnabled.ValueRW = true;
            }
        }
    }
}