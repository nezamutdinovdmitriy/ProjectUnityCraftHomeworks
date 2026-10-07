using Game.Components;
using Game.UseCases;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Systems
{
    [RequireMatchingQueriesForUpdate]
    public partial struct MovementSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach ((RefRO<MovementRequest> request,
                         EnabledRefRW<MovementRequest> enabledRequest,
                         EnabledRefRW<MovementEvent> enabledEvent,
                         RefRO<MovementSpeed> movementSpeed,
                         RefRO<RotationSpeed> rotationSpeed,
                         RefRW<LocalTransform> transform,
                         RefRO<Health> health)
                     in SystemAPI.Query<
                             RefRO<MovementRequest>,
                             EnabledRefRW<MovementRequest>,
                             EnabledRefRW<MovementEvent>,
                             RefRO<MovementSpeed>,
                             RefRO<RotationSpeed>,
                             RefRW<LocalTransform>,
                             RefRO<Health>>()
                         .WithAll<MovementRequest>()
                         .WithPresent<MovementEvent>())
            {
                enabledRequest.ValueRW = false;

                if (health.ValueRO.IsDead())
                    continue;

                MovementUseCase.MoveStep(transform,
                    request.ValueRO.Direction,
                    movementSpeed.ValueRO.Value,
                    deltaTime);

                RotationUseCase.RotationStep(
                    transform,
                    request.ValueRO.Direction,
                    rotationSpeed.ValueRO.Value,
                    deltaTime);

                enabledEvent.ValueRW = true;
            }
        }
    }
}