using System;
using UnityEngine;

namespace Game
{
    // +
    public abstract class Ship : MonoBehaviour, IDamageable
    {
        public event Action<Ship> Fired;
        
        protected readonly HealthComponent HealthComponent = new HealthComponent();
        
        [SerializeField] protected ShipConfig ShipConfig;
        
        [SerializeField] protected RigidbodyMovementComponent RigidbodyMovementComponent;
        
        [SerializeField] private TeamType _team;
        
        public TeamType Team => _team;
        
        public HealthComponent Health => HealthComponent;
        
        public Vector3 MoveDirection { get; protected set; }

        [Header("Combat")]
        public Transform FirePoint;
        public float BulletSpeed;
        public int BulletDamage;
        
        private float _fireTime;


        protected virtual void Awake()
        {
            HealthComponent.Initialize(ShipConfig.Health);

            HealthComponent.Dead += OnShipDestroyed;    
            
            RigidbodyMovementComponent.SetSpeed(ShipConfig.MoveSpeed);
        }

        protected virtual void FixedUpdate()
        {
            if(HealthComponent.Current > 0)
                RigidbodyMovementComponent.MoveStep(MoveDirection);
        }
        
        protected virtual void OnEnable() => HealthComponent.Dead += OnShipDestroyed;
        protected virtual void OnDisable() => HealthComponent.Dead -= OnShipDestroyed;

        public void TakeDamage(int damage) => HealthComponent.TakeDamage(damage);

        public void Fire()
        {
            if(HealthComponent.Current <= 0)
                return;

            float time = Time.time;
            
            if (time - _fireTime < ShipConfig.FireCooldown)
                return;

            _fireTime = time;
            
            Debug.Log($"FRAME: {Time.frameCount} | SCRIPT_ID: {this.GetHashCode()} | FIRE_TIME: {_fireTime}");
            
            Fired?.Invoke(this);
        }

        public void ResetHealth() => HealthComponent.Initialize(ShipConfig.Health);

        private void OnShipDestroyed() => gameObject.SetActive(false);
    }
}