using Atomic.Entities;

namespace Game.GameEntity
{
    public static class HealthUseCase
    {
        public static bool IsDead(this IGameEntity entity) 
            => entity.GetValue(GameEntityAPI.CurrentHealth).Value <= 0;
    }
}