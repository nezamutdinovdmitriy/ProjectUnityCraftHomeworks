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
                         RefRW<LocalTransform> transform)
                     in SystemAPI.Query<
                             RefRO<MovementRequest>,
                             EnabledRefRW<MovementRequest>,
                             EnabledRefRW<MovementEvent>,
                             RefRO<MovementSpeed>,
                             RefRW<LocalTransform>>()
                         .WithAll<MovementRequest>()
                         .WithPresent<MovementEvent>())
            {
                enabledRequest.ValueRW = false;

                MovementUseCase.MoveStep(transform,
                    request.ValueRO.Direction,
                    movementSpeed.ValueRO.Value,
                    deltaTime);

                enabledEvent.ValueRW = true;
            }
        }
    }
}