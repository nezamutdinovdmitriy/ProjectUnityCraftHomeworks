using UnityEngine;

namespace Game
{
    // +
    public sealed class Enemy : Ship
    {
        private IEnemyDespawner _despawner;

        public Ship Target { get; private set; }
        
        protected override void OnEnable()
        {
            base.OnEnable();

            HealthComponent.Dead += OnCharacterDead;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            HealthComponent.Dead -= OnCharacterDead;
        }

        public void SetTarget(Ship target)
        {
            if (TryGetComponent(out EnemyCombatAI combatAI))
                combatAI.Initialize(this, target);

            Target = target;
        }
        
        public void Initialize(
            Vector3 destination,
            IEnemyDespawner despawner
        )
        {
            _despawner = despawner;
            
            ResetHealth();
            
            if(TryGetComponent(out EnemyNavigationAI navigationAI))
                navigationAI.Initialize(this, destination);
        }

        private void OnCharacterDead() => _despawner.Despawn(this);
    }
}