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
                return;

            IVariable<Vector3> position = entity.GetValue(EntityContextAPI.Position);
            IValue<float> speed = entity.GetValue(EntityContextAPI.MovementSpeed);
            
            position.Value += direction * speed.Value * deltaTime;
        }
    }
}