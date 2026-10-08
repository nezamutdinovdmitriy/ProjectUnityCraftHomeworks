using Game.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.UseCases.Systems
{
    [RequireMatchingQueriesForUpdate]
    public partial struct RotationSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (
                         rotationRequest,
                         rotationRequestEnabled,
                         speed,
                         transform)
                     in SystemAPI.Query<
                             RefRO<RotationRequest>,
                             EnabledRefRW<RotationRequest>,
                             RefRO<RotationSpeed>,
                             RefRW<LocalTransform>>()
                         .WithAll<RotationRequest>())
            {
                rotationRequestEnabled.ValueRW = false;

                float3 direction = rotationRequest.ValueRO.Direction;

                RotationUseCase.RotationStep(transform, direction, in speed.ValueRO.Value, deltaTime);
            }
        }
    }
}