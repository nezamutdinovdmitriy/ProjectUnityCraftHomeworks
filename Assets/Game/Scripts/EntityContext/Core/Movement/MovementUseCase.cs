using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.EntityContext
{
    public static class MovementUseCase
    {
        public static void MoveStep(this IEntityContext entity, Vector3 direction, float deltaTime)
        {
            if (direction == Vector3.zero)
            {
                entity.GetValue(EntityContextAPI.IsMoving).Value = false;
                return;
            }

            IVariable<Vector3> position = entity.GetValue(EntityContextAPI.Position);
            IValue<float> speed = entity.GetValue(EntityContextAPI.MovementSpeed);
            entity.GetValue(EntityContextAPI.IsMoving).Value = true;
            
            position.Value += direction * speed.Value * deltaTime;
        }
    }
}