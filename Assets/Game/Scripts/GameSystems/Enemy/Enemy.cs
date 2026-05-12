using UnityEngine;

namespace Game
{
    // +
    public sealed class Enemy : MonoBehaviour
    {
        [SerializeField]
        private Ship _ship;
        
        private IEnemyDespawner _despawner;

        [Header("AI Settings")]
        /*[SerializeField]
        private EnemyNavigationAI _navigation;*/
        
        [SerializeField]
        private Vector2 _destination;
        private float _stoppingDistance = 0.25f;
        
        public Ship Target { get; private set; }
        public bool IsReached { get; private set; }
        
        public Ship Ship { get; private set; }
        
        private void OnEnable()
        {
            _ship.HealthComponent.Dead += OnCharacterDead;
        }

        private void OnDisable()
        {
            _ship.HealthComponent.Dead -= OnCharacterDead;
        }

        public void SetTarget(Ship target)
        {
            /*if (TryGetComponent(out EnemyCombatAI combatAI))
                combatAI.Initialize(this, target);*/

            Target = target;
        }
        
        public void Initialize(
            Vector3 destination,
            IEnemyDespawner despawner
        )
        {
            _despawner = despawner;
            
            _ship.ResetHealth();
            
            /*if(TryGetComponent(out EnemyNavigationAI navigationAI))
                navigationAI.Initialize(this, destination);*/
        }
        
        private void FixedUpdate()
        {
            if (_ship.HealthComponent.IsDead 
                || Target == null 
                || Target.HealthComponent.IsDead)
                return;

            Vector2 vectorToDestination = _destination - (Vector2)transform.position;
            float sqrDistance = vectorToDestination.sqrMagnitude;

            IsReached = sqrDistance <= _stoppingDistance * _stoppingDistance;

            _ship.SetMoveDirection(IsReached ? Vector3.zero : vectorToDestination.normalized);
            
            if(IsReached) 
                _ship.Fire();
        }
        
        private void OnCharacterDead() => _despawner.Despawn(this);
    }
}