using Atomic.Entities;

namespace Game.GameEntities
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