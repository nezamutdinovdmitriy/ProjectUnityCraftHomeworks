using System;
using UnityEngine;

namespace Game
{
    // +
    public abstract class Ship : MonoBehaviour, IDamageable
    {
        public event Action<Ship> Fired;

        public ShipConfig config;

        public TeamType Team => _team;

        protected readonly HealthComponent _healthComponent = new HealthComponent();
        
        public HealthComponent Health => _healthComponent;

        [Header("Combat")]
        
        public Transform firePoint;
        public float bulletSpeed;
        public int bulletDamage;
        private float _fireTime;

        [SerializeField] private TeamType _team;

        [Header("Movement")] [SerializeField]
        
        protected RigidbodyMovementComponent RigidbodyMovementComponent;

        public Vector3 MoveDirection { get; protected set; }


        private void Awake()
        {
            _healthComponent.Initialize(config.Health);

            _healthComponent.Dead += OnShipDestroyed;
            
            RigidbodyMovementComponent.SetSpeed(config.MoveSpeed);
        }

        protected virtual void FixedUpdate() => RigidbodyMovementComponent.FixedUpdate();
        
        private void OnEnable() => _healthComponent.Dead += OnShipDestroyed;
        private void OnDisable() => _healthComponent.Dead -= OnShipDestroyed;

        public void TakeDamage(int damage) => _healthComponent.TakeDamage(damage);

        public void OnFired()
        {
            float time = Time.time;
            if (time - _fireTime < config.FireCooldown || _healthComponent.Current <= 0)
                return;

            _fireTime = time;

            Fired?.Invoke(this);
        }

        public void ResetHealth() => _healthComponent.Initialize(config.Health);

        private void OnShipDestroyed() => gameObject.SetActive(false);
    }
}