using System;
using Modules.UI;
using UnityEngine;

namespace Game
{
    public class PlayerHealthPresenter : MonoBehaviour
    {
        [SerializeField]
        private Ship _ship;
        
        [SerializeField]
        private HealthView _healthView;

        private void OnEnable()
        {
            _ship.HealthComponent.Changed += OnHealthChanged;
            OnHealthChanged(_ship.HealthComponent.Current);
        }

        private void OnDisable() => _ship.HealthComponent.Changed -= OnHealthChanged;

        private void OnHealthChanged(int health)
            => _healthView.SetHealth(health, _ship.HealthComponent.Max);
    }
}