using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.EntityContext.Core.Rotation
{
    public static class RotationUseCase
    {
        public static void RotationStep(this IEntityContext entity, Vector3 direction, float deltaTime)
        {
            if (direction == Vector3.zero)
                return;

            IVariable<Quaternion> rotation = entity.GetValue(EntityContextAPI.Rotation);
            IValue<float> rotationSpeed = entity.GetValue(EntityContextAPI.RotationSpeed);

            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            rotation.Value = Quaternion.RotateTowards(
                rotation.Value,
                targetRotation,
                rotationSpeed.Value * deltaTime);
        }
    }
}