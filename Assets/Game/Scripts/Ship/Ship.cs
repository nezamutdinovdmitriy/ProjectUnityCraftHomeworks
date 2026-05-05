using System;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    // +
    public abstract class Ship : MonoBehaviour, IDamageable
    {
        public event Action<int> HealthChanged;
        public event Action Dead;
        public event Action<Ship> Fired;

        public ShipConfig config;

        public TeamType Team => _team;
        
        [Header("Health")]
        public int currentHealth;
        

        [Header("Combat")]
        public Transform firePoint;
        public float bulletSpeed;
        public int bulletDamage;
        private float _fireTime;

        [SerializeField]
        private TeamType _team;

        [Header("Movement")]
        [SerializeField]
        protected RigidbodyMovementComponent rigidbodyMovementComponent;
        
        protected Vector3 moveDirection;

        public Vector3 MoveDirection => moveDirection;


        private void Awake()
        {
            currentHealth = config.Health;
            rigidbodyMovementComponent.SetSpeed(config.MoveSpeed);
        }
        
        public void TakeDamage(int damage)
        {
            if (damage > 0)
            {
                currentHealth = Mathf.Clamp(currentHealth - damage, 0, config.Health);
                NotifyAboutHealthChanged(currentHealth);
 
                if (currentHealth <= 0)
                {
                    NotifyAboutDead();
                    gameObject.SetActive(false);
                }
            }
        }
        
        public void OnFired()
        {
            float time = Time.time;
            if (time - _fireTime < config.FireCooldown || currentHealth <= 0)
                return;

            _fireTime = time;
            
            this.Fired?.Invoke(this);
        }
        
        protected virtual void FixedUpdate() => rigidbodyMovementComponent.FixedUpdate();
        
        private void NotifyAboutHealthChanged(int health) => HealthChanged?.Invoke(health);
        private void NotifyAboutDead() => Dead?.Invoke();
    }
}