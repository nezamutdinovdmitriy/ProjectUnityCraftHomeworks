using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.UseCases
{
    public static class RotationUseCase
    {
        public static void RotationStep(RefRW<LocalTransform> transform, in float3 direction, in float speed, float deltaTime)
        {
            if (math.lengthsq(direction) <= 0f)
                return;
        
            quaternion target = quaternion.LookRotationSafe(direction, math.up());
        
            float dot = math.abs(math.dot(transform.ValueRO.Rotation.value, target.value));
            dot = math.min(dot, 1f);
        
            float angle = 2f * math.acos(dot);
            float maxStep = math.radians(speed) * deltaTime;
        
            float t = angle < 1e-5f ? 1f : math.min(1f, maxStep / angle);
            transform.ValueRW.Rotation = math.slerp(transform.ValueRO.Rotation, target, t);
        }
        
        // public static void RotationStep(ref quaternion transform, in float3 direction, in float speed, float deltaTime)
        // {
        //     if (math.lengthsq(direction) <= 0f)
        //         return;
        //
        //     quaternion target = quaternion.LookRotationSafe(direction, math.up());
        //
        //     float dot = math.abs(math.dot(transform.value, target.value));
        //     dot = math.min(dot, 1f);
        //
        //     float angle = 2f * math.acos(dot);
        //     float maxStep = math.radians(speed) * deltaTime;
        //
        //     float t = angle < 1e-5f ? 1f : math.min(1f, maxStep / angle);
        //     transform = math.slerp(transform.value, target, t);
        // }
    }
}