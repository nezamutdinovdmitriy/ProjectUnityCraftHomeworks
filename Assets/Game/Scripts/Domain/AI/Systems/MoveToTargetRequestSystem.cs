using Game.Components;
using Game.Components.UseCases;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Systems
{
    public partial struct MoveToTargetRequestSystem : ISystem
    {
        private ComponentLookup<LocalTransform> _transformLookup;
        private ComponentLookup<Health> _healthLookup;

        public void OnCreate(ref SystemState state)
        {
            _transformLookup = SystemAPI.GetComponentLookup<LocalTransform>();
            _healthLookup = SystemAPI.GetComponentLookup<Health>();
        }

        public void OnUpdate(ref SystemState state)
        {
            _healthLookup.Update(ref state);
            _transformLookup.Update(ref state);

            foreach (var (
                         healthRO, 
                         selfTransformRO, 
                         attackDistanceRO, 
                         targetRO,
                         movementRequestRW,
                         movementRequestEnabled)
                     in SystemAPI.Query<
                         RefRO<Health>,
                         RefRO<LocalTransform>,
                         RefRO<AttackDistance>,
                         RefRO<TargetEntity>,
                         RefRW<MovementRequest>,
                         EnabledRefRW<MovementRequest>>()
                         .WithAll<AIControlled>()
                         .WithDisabled<MovementRequest>())
            {
                if(targetRO.ValueRO.Value == Entity.Null
                   || healthRO.ValueRO.IsDead()
                   || _transformLookup.TryGetRefRO(targetRO.ValueRO.Value, out RefRO<LocalTransform> targetTransformRO) == false
                   || _healthLookup.TryGetRefRO(targetRO.ValueRO.Value, out RefRO<Health> targetHealthRO) == false
                   || targetHealthRO.ValueRO.IsDead())
                    continue;

                float3 delta = targetTransformRO.ValueRO.Position - selfTransformRO.ValueRO.Position;
                
                if (AttackUseCase.InAttackDistance(in delta, attackDistanceRO.ValueRO.Value) == false)
                {
                    movementRequestRW.ValueRW.Direction = math.normalizesafe(delta);
                    movementRequestEnabled.ValueRW = true;
                }
            }
        }
    }
}