using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.EntityContext.Core.Health
{
    public static class HealthUseCase
    {
        public static bool IsDead(this IEntityContext entity) 
            => entity.GetValue(EntityContextAPI.CurrentHealth).Value <= 0;

        public static bool ReduceHealth(this IEntityContext entity, int damage)
        {
            if (entity.IsDead())
                return false;
                

            IVariable<float> health = entity.GetValue(EntityContextAPI.CurrentHealth);
            health.Value = Mathf.Max(0, health.Value - damage);

            if(health.Value <= 0)
                entity.GetValue(EntityContextAPI.IsDead).Value = true;
            
            return true;
        }
    }
}