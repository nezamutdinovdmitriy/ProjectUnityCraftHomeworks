using System;
using UnityEngine;

namespace Game
{
    // +
    public abstract class Ship : MonoBehaviour, IDamageable
    {
        [SerializeField]
        protected ShipConfig ShipConfig;

        [SerializeField]
        protected RigidbodyMovementComponent RigidbodyMovementComponent;

        [SerializeField]
        private TeamType _team;

        public TeamType Team => _team;
        public HealthComponent HealthComponent { get; } = new HealthComponent();
        public FireComponent FireComponent { get; } = new FireComponent();
        public Vector3 MoveDirection { get; protected set; }

        [Header("Combat")]
        public Transform FirePoint;
        public float BulletSpeed;
        public int BulletDamage;

        protected virtual void Awake()
        {
            HealthComponent.Initialize(ShipConfig.Health);

            FireComponent.Initialize(ShipConfig.FireCooldown);

            RigidbodyMovementComponent.SetSpeed(ShipConfig.MoveSpeed);
        }

        protected virtual void FixedUpdate()
        {
            if (HealthComponent.Current > 0)
                RigidbodyMovementComponent.MoveStep(MoveDirection);
        }

        protected virtual void OnEnable() => HealthComponent.Dead += OnShipDestroyed;
        protected virtual void OnDisable() => HealthComponent.Dead -= OnShipDestroyed;

        public void TakeDamage(int damage) => HealthComponent.TakeDamage(damage);

        public void SetMoveDirection(Vector2 direction) => MoveDirection = direction;
        
        public void Fire() => FireComponent.Execute(this, HealthComponent.IsDead == false);
        
        public void ResetHealth() => HealthComponent.Initialize(ShipConfig.Health);

        private void OnShipDestroyed() => gameObject.SetActive(false);
    }
}