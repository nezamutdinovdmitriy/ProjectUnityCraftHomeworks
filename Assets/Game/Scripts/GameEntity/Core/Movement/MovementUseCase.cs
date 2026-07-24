using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntity
{
    public static class MovementUseCase
    {
        public static void MoveStep(this IGameEntity entity, Vector3 direction, float speed, float deltaTime) 
            => entity.GetValue(GameEntityAPI.Position).Value += direction * speed * deltaTime;
    }
}