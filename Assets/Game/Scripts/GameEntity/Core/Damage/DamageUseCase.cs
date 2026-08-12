using Atomic.Entities;

namespace Game.GameEntity
{
    public static class DamageUseCase
    {
        public static bool TryInvokeTakeDamageCommand(this IGameEntity entity, float damage)
        {
            if (entity.HasTag(GameEntityAPI.DamageableTag) == false)
                return false;

            entity.GetValue(GameEntityAPI.TakeDamageCommand).Invoke(damage);
            return true;
        }
    }
}