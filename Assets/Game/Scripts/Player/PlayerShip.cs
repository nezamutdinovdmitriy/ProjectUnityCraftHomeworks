using Modules.UI;
using Modules.Utils;
using UnityEngine;

namespace Game
{
    // +
    public sealed class PlayerShip : Ship
    {
        [SerializeField]
        private TransformBounds _playerArea;
        [SerializeField]
        private CameraShaker _cameraShaker;

        [Header("UI")] [SerializeField]
        private GameOverView _gameOverView;
        [SerializeField]
        private HealthView _healthView;

        protected override void OnEnable()
        {
            base.OnEnable();

            HealthComponent.Changed += OnHealthChanged;
            HealthComponent.Dead += _gameOverView.Show;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            HealthComponent.Changed -= OnHealthChanged;
            HealthComponent.Dead -= _gameOverView.Show;
        }

        public void SetMovementDirection(Vector2 direction) => MoveDirection = direction;
        
        

        private void OnHealthChanged(int health)
        {
            _healthView.SetHealth(health, ShipConfig.Health);
            _cameraShaker.Shake();
        }
    }
}