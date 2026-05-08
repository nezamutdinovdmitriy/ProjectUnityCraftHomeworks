using Modules.UI;
using UnityEngine;

namespace Game
{
    public class PlayerUIController : MonoBehaviour
    {
        [SerializeField]
        private Ship _ship;

        [SerializeField]
        private HealthView _healthView;

        [SerializeField]
        private GameOverView _gameOverView;

        private void OnEnable()
        {
            _ship.HealthComponent.Changed += OnHealthChanged;
            _ship.HealthComponent.Dead += _gameOverView.Show;
        }

        private void OnDisable()
        {
            _ship.HealthComponent.Changed -= OnHealthChanged;
            _ship.HealthComponent.Dead -= _gameOverView.Show;
        }
        
        private void OnHealthChanged(int health)
            => _healthView.SetHealth(health, _ship.HealthComponent.Max);
    }
}