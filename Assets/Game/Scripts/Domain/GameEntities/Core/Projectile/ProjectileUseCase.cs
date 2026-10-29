using Game.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.UseCases
{
    public static class ProjectileUseCase
    {
        public static Entity Create(
            ref EntityCommandBuffer.ParallelWriter commands,
            int sortKey,
            Entity prefab,
            Entity target,
            in LocalToWorld spawnTransform,
            in LocalToWorld targetTransform,
            in LocalTransform prefabTransform)
        {
            float3 direction = math.normalizesafe(
                targetTransform.Position + new float3(0f, 0.5f, 0f) - spawnTransform.Position,
                math.normalizesafe(spawnTransform.Forward, new float3(0f, 0f, 1f)));

            LocalTransform transform = prefabTransform;

            transform.Position = spawnTransform.Position;
            transform.Rotation = math.mul(
                quaternion.LookRotationSafe(direction, math.up()),
                transform.Rotation);

            Entity projectile = commands.Instantiate(sortKey, prefab);
            
            commands.SetComponent(sortKey, projectile, transform);
            
            commands.SetComponent(sortKey, projectile, new TargetEntity
            {
                Value = target
            });
            
            commands.SetComponent(sortKey, projectile, new MovementRequest
            {
                Direction = direction
            });

            commands.SetComponentEnabled<MovementRequest>(sortKey, projectile, true);

            commands.SetComponent(sortKey, projectile, new RotationRequest
            {
                Direction = direction
            });
                
            commands.SetComponentEnabled<RotationRequest>(sortKey, projectile, true);

            return projectile;
        }
    }
}