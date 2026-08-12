using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntities
{
    public static class HealthUseCase
    {
        public static bool IsDead(this IGameEntity entity) 
            => entity.GetValue(GameEntityAPI.CurrentHealth).Value <= 0;

        public static void HealthReduce(this IGameEntity entity, float value)
        {
            IReactiveVariable<float> currentHealth = entity.GetValue(GameEntityAPI.CurrentHealth);
            
            currentHealth.Value = Mathf.Max(0, currentHealth.Value - value);
        }

        public static void HealthRestore(this IGameEntity entity, float value)
        {
            IReactiveVariable<float> currentHealth = entity.GetValue(GameEntityAPI.CurrentHealth);
            float maxHealth = entity.GetValue(GameEntityAPI.MaxHealth).Value;

            currentHealth.Value = Mathf.Min(currentHealth.Value + value, maxHealth);
        }
        
        public static bool IsHealthNotFull(this IGameEntity entity)
        {
            float maxHealth = entity.GetValue(GameEntityAPI.MaxHealth).Value;
            float currentHealth = entity.GetValue(GameEntityAPI.CurrentHealth).Value;

            return currentHealth < maxHealth;
        }
    }
}