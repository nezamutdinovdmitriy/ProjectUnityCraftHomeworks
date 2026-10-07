using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.UseCases
{
    public static class MovementUseCase
    {
        public static void MoveStep(
            RefRW<LocalTransform> transform, 
            in float3 direction, 
            in float speed, 
            float deltaTime)
        {
            transform.ValueRW.Position += direction * speed * deltaTime;
        }
    }
}