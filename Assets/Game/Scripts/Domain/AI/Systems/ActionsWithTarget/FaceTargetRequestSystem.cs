using Game.Components;
using Game.UseCases;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Systems
{
    public partial struct FaceTargetRequestSystem : ISystem
    {
        private ComponentLookup<Health> _healthLookup;
        private ComponentLookup<LocalTransform> _transformLookup;

        public void OnCreate(ref SystemState state)
        {
            _healthLookup = SystemAPI.GetComponentLookup<Health>();
            _transformLookup = SystemAPI.GetComponentLookup<LocalTransform>();
        }

        public void OnUpdate(ref SystemState state)
        {
            _healthLookup.Update(ref state);
            _transformLookup.Update(ref state);
            
            foreach (var (
                         transform,
                         health,
                         target,
                         rotationRequest,
                         rotationRequestEnabled)
                     in SystemAPI.Query<
                             RefRW<LocalTransform>,
                             RefRO<Health>,
                             RefRO<TargetEntity>,
                             RefRW<RotationRequest>,
                             EnabledRefRW<RotationRequest>>()
                         .WithAll<AIControlled>()
                         .WithDisabled<RotationRequest>())
            {
                if(target.ValueRO.Value == Entity.Null
                   || health.ValueRO.IsDead()
                   || _transformLookup.TryGetRefRO(target.ValueRO.Value, out RefRO<LocalTransform> targetTransform) == false
                   || _healthLookup.TryGetRefRO(target.ValueRO.Value, out RefRO<Health> targetHealth) == false
                   || targetHealth.ValueRO.IsDead())
                    continue;
                
                float3 delta = targetTransform.ValueRO.Position - transform.ValueRO.Position;
                delta.y = 0;

                rotationRequest.ValueRW.Direction = math.normalizesafe(delta);
                rotationRequestEnabled.ValueRW = true;
            }
        }
    }
}