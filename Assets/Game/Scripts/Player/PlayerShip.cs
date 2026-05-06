using Modules.UI;
using Modules.Utils;
using UnityEngine;

namespace Game
{
    // +
    public sealed class PlayerShip : Ship
    {
        [SerializeField] private TransformBounds _playerArea;

        [SerializeField] private CameraShaker _cameraShaker;

        [Header("UI")]
        
        [SerializeField] private GameOverView _gameOverView;
        [SerializeField] private HealthView _healthView;

        private void OnEnable()
        {
            Health.Changed += OnHealthChanged;
            Health.Dead += _gameOverView.Show;
        }

        private void OnDisable()
        {
            Health.Changed -= OnHealthChanged;
            Health.Dead -= _gameOverView.Show;
        }

        private void LateUpdate() 
            => transform.position = _playerArea.ClampInBounds(transform.position);
        
        public void SetMovementDirection(Vector2 direction) => MoveDirection = direction;
        
        public void Update()
        {
            if (_healthComponent.Current > 0)
                RigidbodyMovementComponent.MoveStep(MoveDirection);
        }

        private void OnHealthChanged(int health)
        {
            _healthView.SetHealth(health, config.Health);
            _cameraShaker.Shake();
        }
    }
}