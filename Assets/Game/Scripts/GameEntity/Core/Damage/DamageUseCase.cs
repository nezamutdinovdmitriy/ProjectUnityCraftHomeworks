using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntity
{
    public static class DamageUseCase
    {
        public static bool TakeDamage(this IGameEntity entity, float damage)
        {
            if (entity.HasTag(GameEntityAPI.DamageableTag) == false)
                return false;

            Debug.Log($"{damage} applied!");
            entity.GetValue(GameEntityAPI.TakeDamageCommand).Invoke(damage);
            return true;
        }
    }
}