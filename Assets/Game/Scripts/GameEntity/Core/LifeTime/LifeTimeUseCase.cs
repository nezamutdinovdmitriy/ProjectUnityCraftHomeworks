using Atomic.Entities;

namespace Game.GameEntity.Core.LifeTime
{
    public static class LifeTimeUseCase
    {
        public static void ExpireLifetime(this IGameEntity entity)
        {
            entity.GetValue(GameEntityAPI.Lifetime).ResetTime();
            entity.GetValue(GameEntityAPI.DestroyAction).Invoke();
        }
    }
}