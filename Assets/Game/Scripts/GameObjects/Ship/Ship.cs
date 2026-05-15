using UnityEngine;

namespace Game
{
    // +
    public class Ship : MonoBehaviour, IDamageable
    {
        [SerializeField]
        protected ShipConfig ShipConfig;

        [SerializeField]
        protected MovementComponent movementComponent;

        [SerializeField]
        private TeamType _team;

        public TeamType Team => _team;
        
        [field: SerializeField]
        public FireComponent FireComponent { get; private set; }
        public HealthComponent HealthComponent { get; } = new HealthComponent();
        public Vector3 MoveDirection { get; protected set; }

        [Header("Combat")]
        public Transform FirePoint;

        protected virtual void Awake()
        {
            HealthComponent.SetMaxHealth(ShipConfig.Health);
            HealthComponent.SetCurrentHealth(ShipConfig.Health);

            FireComponent.SetCooldown(ShipConfig.FireCooldown);
            
            movementComponent.SetSpeed(ShipConfig.MoveSpeed);
        }
        
        protected virtual void OnEnable() => HealthComponent.Dead += OnShipDestroyed;
        protected virtual void OnDisable() => HealthComponent.Dead -= OnShipDestroyed;
        
        protected virtual void FixedUpdate()
        {
            if (HealthComponent.Current > 0)
                movementComponent.MoveStep(MoveDirection, Time.fixedDeltaTime);
        }

        public void Initialize(BulletManager bulletManager)
        {
            FireComponent.Construct(bulletManager);
            FireComponent.SetCooldown(ShipConfig.FireCooldown);
        }
        
        public void TakeDamage(int damage) => HealthComponent.TakeDamage(damage);

        public void SetMoveDirection(Vector2 direction) => MoveDirection = direction;
        
        public void Fire(Vector2 direction) 
            => FireComponent.Execute(this, direction, HealthComponent.IsDead == false);
        
        public void ResetHealth() => HealthComponent.SetCurrentHealth(ShipConfig.Health);

        private void OnShipDestroyed() => gameObject.SetActive(false);
    }
}