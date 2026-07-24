using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntity
{
    public static class RotateUseCase
    {
        public static void RotateStep(this IGameEntity entity, Vector3 direction, float speed, float deltaTime)
        {
            TransformRotationVariable rotation = entity.GetValue(GameEntityAPI.Rotation);
            //IValue<float> rotationSpeed = entity.GetValue(GameEntityAPI.RotateSpeed);
            
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            rotation.Value = Quaternion.RotateTowards(
                rotation.Value,
                targetRotation,
                speed * deltaTime);
        }
    }
}