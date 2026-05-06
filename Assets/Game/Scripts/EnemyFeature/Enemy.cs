using UnityEngine;

namespace Game
{
    // +
    public sealed class Enemy : Ship
    {
        [Header("AI Settings")]
        public Ship target;
        public Vector2 destination;

        [SerializeField] private float _stoppingDistance = 0.25f;

        private IEnemyDespawner _despawner;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            Health.Dead += OnCharacterDead;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            Health.Dead -= OnCharacterDead;
        }
        
        public void SetDespawner(IEnemyDespawner despawner) => _despawner = despawner;

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (Health.Current <= 0 || target == null || target.Health.Current <= 0)
                return;

            Vector2 distance = destination - (Vector2)transform.position;
            bool isNotReached = distance.sqrMagnitude > _stoppingDistance * _stoppingDistance;
            
            MoveDirection = isNotReached ? distance.normalized : Vector3.zero;
            
            Debug.Log($"isNotReached {isNotReached}");
            
            if (isNotReached)
                RigidbodyMovementComponent.MoveStep(distance.normalized);
            else
                Fire();
        }
        
        private void OnCharacterDead() => _despawner.Despawn(this);
    }
}