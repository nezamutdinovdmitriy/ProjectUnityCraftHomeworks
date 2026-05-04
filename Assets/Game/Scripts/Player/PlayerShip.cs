using Modules.UI;
using Modules.Utils;
using UnityEngine;

namespace Game
{
    // +
    public sealed class PlayerShip : ShipController
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
            OnDead += _gameOverView.Show;
        }

        private void OnDisable()
        {
            HealthChanged -= OnHealthChanged;
            OnDead -= _gameOverView.Show;
        }

        private void OnHealthChanged(int health)
        {
            _healthView.SetHealth(health, config.Health);
            _cameraShaker.Shake();
        }
        
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Fire();

            float dx = Input.GetAxisRaw("Horizontal");
            float dy = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector2(dx, dy);

            if (currentHealth > 0)
            {
                _motor.MoveStep(moveDirection);
            }
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            this.transform.position = _playerArea.ClampInBounds(transform.position);
        }
    }
}