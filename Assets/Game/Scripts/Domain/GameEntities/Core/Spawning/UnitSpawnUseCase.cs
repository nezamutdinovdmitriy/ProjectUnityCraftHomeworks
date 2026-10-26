using Game.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.UseCases
{
    public static class UnitSpawnUseCase
    {
        public static LocalTransform GetTransform(
            in LocalToWorld point,
            in LocalTransform prefabTransform)
        {
            LocalTransform transform = prefabTransform;

            transform.Position = point.Position;
            transform.Rotation = math.mul(point.Rotation, prefabTransform.Rotation);

            return transform;
        }

        public static Entity Create(
            ref EntityCommandBuffer commands,
            Entity prefab,
            in LocalTransform transform,
            TeamType team,
            Entity defaultTarget)
        {
            Entity unit = commands.Instantiate(prefab);

            commands.SetComponent(unit, transform);
            commands.SetComponent(unit, new Team
            {
                Value = team
            });

            if (defaultTarget != Entity.Null)
            {
                commands.SetComponent(unit, new DefaultTargetEntity
                {
                    Value = defaultTarget
                });
            }

            return unit;
        }
    }
}