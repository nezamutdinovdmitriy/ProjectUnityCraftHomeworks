using System;
using Atomic.Entities;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.GameEntity
{
    public static class DamageUseCase
    {
        public static bool TryTakeDamage(this IGameEntity entity, float damage)
        {
            if (entity.HasTag(GameEntityAPI.DamageableTag) == false)
                return false;

            Debug.Log($"{damage} applied!");
            entity.GetValue(GameEntityAPI.TakeDamageCommand).Invoke(damage);
            return true;
        }

        public static async UniTaskVoid TryTakeDamageDelayed(
            this IGameEntity entity,
            float damage,
            float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            entity.TryTakeDamage(damage);
        }
    }
}