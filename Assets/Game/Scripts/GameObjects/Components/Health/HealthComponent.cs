using System;
using UnityEngine;

namespace Game
{
    public sealed class HealthComponent : MonoBehaviour
    {
        [SerializeField]
        private float _maxHealth;

        public float CurrentHealth { get; private set; }
        public bool IsAlive => CurrentHealth > 0;
        public bool IsDied => CurrentHealth <= 0;

        public event Action<float> OnHealthChanged;
        public event Action OnDied;

        private void Awake()
        {
            CurrentHealth = _maxHealth;
        }

        public void TakeDamage(float damage)
        {
            if (!IsAlive || damage <= 0)
                return;

            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
            OnHealthChanged?.Invoke(CurrentHealth);

            if (CurrentHealth <= 0)
                OnDied?.Invoke();
        }

        public void SetZero() => 
            this.TakeDamage(this.CurrentHealth);
    }
}