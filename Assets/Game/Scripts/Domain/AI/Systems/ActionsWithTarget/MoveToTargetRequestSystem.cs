using Game.Components;
using Game.ECS.SystemGroups;
using Game.UseCases;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Systems
{
    [UpdateInGroup(typeof(GenerateRequestsSystemGroup))]
    [BurstCompile]
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

        public void OnUpdate(ref SystemState state)
        {
            _definitionLookup.Update(ref state);
            _healthLookup.Update(ref state);
            _transformLookup.Update(ref state);

            foreach (var (
                         health,
                         selfTransform,
                         selectedAbility,
                         target,
                         movementRequest,
                         movementRequestEnabled)
                     in SystemAPI.Query<
                             RefRO<Health>,
                             RefRO<LocalTransform>,
                             RefRO<SelectedAbility>,
                             RefRO<TargetEntity>,
                             RefRW<MovementRequest>,
                             EnabledRefRW<MovementRequest>>()
                         .WithAll<AIControlled>()
                         .WithDisabled<MovementRequest>())
            {
                if (health.ValueRO.IsDead())
                    continue;

                Entity targetEntity = target.ValueRO.Value;

                if (targetEntity == Entity.Null)
                    continue;

                if (_definitionLookup.TryGetRefRO(
                        selectedAbility.ValueRO.Value,
                        out RefRO<AbilityDefinition> definition) == false)
                    continue;

                if (_transformLookup.TryGetRefRO(
                        targetEntity,
                        out RefRO<LocalTransform> targetTransform) == false)
                    continue;

                if (_healthLookup.TryGetRefRO(
                        targetEntity,
                        out RefRO<Health> targetHealth) == false
                    || targetHealth.ValueRO.IsDead())
                    continue;

                float3 delta = targetTransform.ValueRO.Position - selfTransform.ValueRO.Position;

                float distanceSq = math.lengthsq(delta);
                float rangeSq = definition.ValueRO.Range * definition.ValueRO.Range;

                if (distanceSq <= rangeSq)
                    continue;

                movementRequest.ValueRW.Direction = math.normalizesafe(delta);
                movementRequestEnabled.ValueRW = true;
            }
        }
    }
}