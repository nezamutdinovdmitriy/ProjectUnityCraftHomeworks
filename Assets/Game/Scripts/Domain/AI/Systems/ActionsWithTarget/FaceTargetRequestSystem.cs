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
    public partial struct FaceTargetRequestSystem : ISystem
    {
        private ComponentLookup<Health> _healthLookup;
        private ComponentLookup<LocalTransform> _transformLookup;

        public void OnCreate(ref SystemState state)
        {
            _healthLookup = SystemAPI.GetComponentLookup<Health>(true);
            _transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _healthLookup.Update(ref state);
            _transformLookup.Update(ref state);

            state.Dependency = new GenerateRotationRequestJob
            {
                HealthLookup = _healthLookup,
                TransformLookup = _transformLookup
            }.ScheduleParallel(state.Dependency);
        }
        
        [BurstCompile]
        [WithAll(typeof(AIControlled))]
        [WithDisabled(typeof(RotationRequest))]
        private partial struct GenerateRotationRequestJob : IJobEntity
        {
            [ReadOnly] public ComponentLookup<Health> HealthLookup;
            [ReadOnly] public ComponentLookup<LocalTransform> TransformLookup;
            
            private void Execute(
                ref RotationRequest request,
                in LocalTransform selfTransform,
                in Health selfHealth,
                in TargetEntity target,
                EnabledRefRW<RotationRequest> requestEnabled)
            {
                Entity targetEntity = target.Value;

                if (targetEntity == Entity.Null || selfHealth.IsDead())
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
                delta.y = 0;

                request.Direction = math.normalizesafe(delta);
                requestEnabled.ValueRW = true;
            }
        }
    }
}