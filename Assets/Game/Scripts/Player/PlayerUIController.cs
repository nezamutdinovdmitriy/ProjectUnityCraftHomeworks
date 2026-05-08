using Modules.UI;
using UnityEngine;

namespace Game
{
    public class PlayerUIController : MonoBehaviour
    {
        [SerializeField]
        private PlayerShip _player;

        [SerializeField]
        private HealthView _healthView;

        [SerializeField]
        private GameOverView _gameOverView;

        private void OnEnable()
        {
            _player.HealthComponent.Changed += OnHealthChanged;
            _player.HealthComponent.Dead += _gameOverView.Show;
        }

        private void OnDisable()
        {
            _player.HealthComponent.Changed -= OnHealthChanged;
            _player.HealthComponent.Dead -= _gameOverView.Show;
        }
        
        private void OnHealthChanged(int health)
            => _healthView.SetHealth(health, _player.HealthComponent.Max);
    }
}