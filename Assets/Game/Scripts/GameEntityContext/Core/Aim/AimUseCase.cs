using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntities
{
    public static class AimUseCase
    {
        public static bool IsAimDelayCompleted(this IGameEntity entity) 
            => entity.GetValue(GameEntityAPI.AimCooldown).IsCompleted();

        public static bool IsAiming(this IGameEntity entity, Vector3 direction) 
            => direction != Vector3.zero;

        public static void AimProcess(
            this IGameEntity entity, 
            IVariable<Vector3> aimDirection, 
            IVariable<bool> hasAimingLastFrame, 
            ICooldown cooldown, 
            IRequest fireRequest, 
            float deltaTime)
        {
            bool isAiming = entity.IsAiming(aimDirection.Value);

            if (isAiming)
            {
                if (hasAimingLastFrame.Value == false)
                    cooldown.ResetTime();

                cooldown.Tick(deltaTime);

                if (cooldown.IsCompleted())
                    fireRequest.Invoke();
            }

            hasAimingLastFrame.Value = isAiming;
        }
    }
}