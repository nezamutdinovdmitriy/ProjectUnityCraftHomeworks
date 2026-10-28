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
    public partial struct MoveToTargetRequestSystem : ISystem
    {
        private ComponentLookup<LocalTransform> _transformLookup;
        private ComponentLookup<Health> _healthLookup;
        private ComponentLookup<AbilityDefinition> _definitionLookup;

        public void OnCreate(ref SystemState state)
        {
            _transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);
            _healthLookup = SystemAPI.GetComponentLookup<Health>(true);
            _definitionLookup = SystemAPI.GetComponentLookup<AbilityDefinition>(true);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _definitionLookup.Update(ref state);
            _healthLookup.Update(ref state);
            _transformLookup.Update(ref state);

            state.Dependency = new GenerateMoveRequestJob
            {
                TransformLookup = _transformLookup,
                HealthLookup = _healthLookup,
                DefinitionLookup = _definitionLookup,
            }.ScheduleParallel(state.Dependency);
        }
        
        [BurstCompile]
        [WithAll(typeof(AIControlled))]
        [WithDisabled(typeof(MovementRequest))]
        private partial struct GenerateMoveRequestJob : IJobEntity
        {
            [ReadOnly] public ComponentLookup<LocalTransform> TransformLookup;
            [ReadOnly] public ComponentLookup<Health> HealthLookup;
            [ReadOnly] public ComponentLookup<AbilityDefinition> DefinitionLookup;
            
            private void Execute(
                in Health selfHealth,
                in LocalTransform selfTransform,
                in SelectedAbility selectedAbility,
                in TargetEntity target,
                ref MovementRequest request,
                EnabledRefRW<MovementRequest> requestEnabled)
            {
                if (selfHealth.IsDead())
                    return;

                Entity targetEntity = target.Value;

                if (targetEntity == Entity.Null)
                    return;

                if (DefinitionLookup.TryGetRefRO(
                        selectedAbility.Value,
                        out RefRO<AbilityDefinition> definition) == false)
                    return;

                if (TransformLookup.TryGetRefRO(
                        targetEntity,
                        out RefRO<LocalTransform> targetTransform) == false)
                    return;

                if (HealthLookup.TryGetRefRO(
                        targetEntity,
                        out RefRO<Health> targetHealth) == false
                    || targetHealth.ValueRO.IsDead())
                    return;

                float3 delta = targetTransform.ValueRO.Position - selfTransform.Position;
                float distanceSq = math.lengthsq(delta);
                float rangeSq = definition.ValueRO.Range * definition.ValueRO.Range;

                if (distanceSq <= rangeSq)
                    return;

                request.Direction = math.normalizesafe(delta);
                requestEnabled.ValueRW = true;
            }
        }
    }
}