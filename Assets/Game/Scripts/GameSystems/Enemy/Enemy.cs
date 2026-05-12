using UnityEngine;

namespace Game
{
    // +
    public sealed class Enemy : MonoBehaviour
    {
        [SerializeField]
        private Ship _ship;

        private IEnemyDespawner _despawner;

        [Header("AI Settings")] [SerializeField]
        private Vector2 _destination;
        private float _stoppingDistance = 0.25f;

        public Ship Target { get; private set; }
        public bool IsReached { get; private set; }
        public Ship Ship => _ship;

        private void Start() => _ship.HealthComponent.Dead += OnCharacterDead;
        private void OnDisable() => _ship.HealthComponent.Dead -= OnCharacterDead;

        private void FixedUpdate()
        {
            if (_ship.HealthComponent.IsDead
                || Target == null
                || Target.HealthComponent.IsDead)
                return;

            Move();

            Vector2 direction = (Target.transform.position - _ship.FirePoint.position).normalized;
            
            Fire(direction);
        }
        
        public void Initialize(Vector3 destination, BulletManager bulletManager, IEnemyDespawner despawner)
        {
            _despawner = despawner;
            _ship.ResetHealth();
        }

        public void SetTarget(Ship target) => Target = target;

        private void Move()
        {
            Vector2 vectorToDestination = _destination - (Vector2) transform.position;
            float sqrDistance = vectorToDestination.sqrMagnitude;

            IsReached = sqrDistance <= _stoppingDistance * _stoppingDistance;

            _ship.SetMoveDirection(IsReached ? Vector3.zero : vectorToDestination.normalized);
        }

        private void Fire(Vector2 direction)
        {
            if (IsReached)
                _ship.Fire(direction);
        }
        
        private void OnCharacterDead() => _despawner.Despawn(this);
        
    }
}