using Game.Components;
using Game.Scripts.Domain.GameEntities.Core.Movement;
using Game.Scripts.Domain.GameEntities.Core.Rotation;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Systems
{
    public partial struct MovementSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
            => state.RequireForUpdate<MovementRequest>();

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
                         .WithPresent<MovementRequest>())
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