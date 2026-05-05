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

        [Header("UI")]
        [SerializeField]
        private GameOverView _gameOverView;

        [SerializeField]
        private HealthView _healthView;

        private void OnEnable()
        {
            HealthChanged += OnHealthChanged;
            Dead += _gameOverView.Show;
        }

        private void OnDisable()
        {
            HealthChanged -= OnHealthChanged;
            Dead -= _gameOverView.Show;
        }

        private void OnHealthChanged(int health)
        {
            _healthView.SetHealth(health, config.Health);
            _cameraShaker.Shake();
        }

        public void SetMovementDirection(Vector2 direction) => moveDirection = direction;
        
        public void Update()
        {
            if (currentHealth > 0)
                rigidbodyMovementComponent.MoveStep(moveDirection);
        }

        private void LateUpdate() 
            => transform.position = _playerArea.ClampInBounds(transform.position);
    }
}