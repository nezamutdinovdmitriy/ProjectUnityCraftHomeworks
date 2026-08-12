using Atomic.Entities;

namespace Game.GameEntity
{
    public static class AimUseCase
    {
        public static bool IsAimDelayCompleted(this IGameEntity entity)
        {
            return entity.GetValue(GameEntityAPI.AimCooldown).IsCompleted();
        }
    }
}