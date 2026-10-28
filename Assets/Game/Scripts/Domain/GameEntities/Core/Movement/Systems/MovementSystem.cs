using Game.Components;
using Game.UseCases;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Systems
{
    [RequireMatchingQueriesForUpdate]
    public partial struct MovementSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            state.Dependency = new MovementJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime
            }.ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        [WithPresent(typeof(MovementEvent))]
        private partial struct MovementJob : IJobEntity
        {
            public float DeltaTime;
            
            private void Execute(
                ref MovementRequest request,
                EnabledRefRW<MovementRequest> requestEnabled,
                EnabledRefRW<MovementEvent> movementEvent,
                in MovementSpeed speed,
                ref LocalTransform transform)
            {
                requestEnabled.ValueRW = false;

                MovementUseCase.MoveStep(
                    ref transform,
                    request.Direction,
                    speed.Value,
                    DeltaTime);

                movementEvent.ValueRW = true;
            }
        }
    }
}