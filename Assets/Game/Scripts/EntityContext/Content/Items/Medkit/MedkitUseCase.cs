using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.EntityContext.Items.Medkit
{
    public static class MedkitUseCase
    {
        public static void CollectMedkit(this IEntityContext interactor, float amount)
        {
            IReactiveVariable<float> currentHealth = interactor.GetValue(EntityContextAPI.CurrentHealth);
            IValue<float> maxHealth = interactor.GetValue(EntityContextAPI.MaxHealth);
            currentHealth.Value = Mathf.Min(currentHealth.Value + amount, maxHealth.Value);
        }
    }
}