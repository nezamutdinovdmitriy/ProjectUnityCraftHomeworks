using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameObjects.Components
{
    public sealed class HealthComponent
    {
        public event Action<float> OnHealthChanged;
        public event Action OnDied;

        public HealthComponent(float maxHealth) => CurrentHealth = maxHealth;
        
        public float CurrentHealth { get; private set; }
        public bool IsAlive => CurrentHealth > 0;
        public bool IsDied => CurrentHealth <= 0;
        
        [Button]
        public void TakeDamage(float damage)
        {
            if (!IsAlive || damage <= 0)
                return;

            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
            OnHealthChanged?.Invoke(CurrentHealth);

            if (CurrentHealth <= 0)
                OnDied?.Invoke();
        }

        [Button]
        public void SetZero() => 
            this.TakeDamage(this.CurrentHealth);
    }
}