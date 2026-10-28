using Game.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Game.UseCases.Systems
{
    [RequireMatchingQueriesForUpdate]
    public partial struct RotationSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new RotationJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime
            }.ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        private partial struct RotationJob : IJobEntity
        {
            public float DeltaTime;
            
            private void Execute(
                ref RotationRequest request,
                EnabledRefRW<RotationRequest> requestEnabled,
                in RotationSpeed speed,
                ref LocalTransform transform)
            {
                requestEnabled.ValueRW = false;
                
                RotationUseCase.RotationStep(ref transform, request.Direction, speed.Value, DeltaTime);
            }
        }
    }
}