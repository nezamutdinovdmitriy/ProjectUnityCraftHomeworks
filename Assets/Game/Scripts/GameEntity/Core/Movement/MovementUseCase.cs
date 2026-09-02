using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntities
{
    public static class MovementUseCase
    {
        public static void MoveStep(this IGameEntity entity, Vector3 direction, float speed, float deltaTime)
        {
            entity.GetValue(GameEntityAPI.Position).Value += direction * speed * deltaTime;
        }

        public static void MoveStepForward(this IGameEntity entity, float speed, float deltaTime)
        {
            Quaternion rotation = entity.GetValue(GameEntityAPI.Rotation).Value;

            entity.MoveStep(rotation * Vector3.forward, speed, deltaTime);
        }
        
        public static void MoveStepForward(this IGameEntity entity, float deltaTime)
        {
            Quaternion rotation = entity.GetValue(GameEntityAPI.Rotation).Value;
            float speed = entity.GetValue(GameEntityAPI.MovementSpeed).Value;

            entity.MoveStep(rotation * Vector3.forward, speed, deltaTime);
        }
        
        public static void FollowToTarget(this IGameEntity entity, IGameEntity target, float stoppingDistance)
        {
            Vector3 targetPosition = target.GetValue(GameEntityAPI.Position).Value;
            Vector3 selfPosition = entity.GetValue(GameEntityAPI.Position).Value;
            IVariable<bool> isReached = entity.GetValue(GameEntityAPI.TargetIsReached);
            
            Vector3 moveDirection = (targetPosition - selfPosition).normalized;
            
            isReached.Value = (targetPosition - selfPosition).magnitude <= stoppingDistance;
            
            if(isReached.Value == false)
                entity.GetValue(GameEntityAPI.MovementRequest).Invoke(moveDirection);
            
            entity.GetValue(GameEntityAPI.RotateRequest).Invoke(moveDirection);
        }
    }
}