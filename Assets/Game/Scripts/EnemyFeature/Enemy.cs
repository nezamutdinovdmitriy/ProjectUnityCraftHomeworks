using UnityEngine;

namespace Game
{
    // +
    public sealed class Enemy : Ship
    {
        private IEnemyDespawner _despawner;

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

        public void Initialize(
            Ship target,
            Vector3 destination,
            IEnemyDespawner despawner
        )
        {
            _despawner = despawner;
            
            ResetHealth();

            if (TryGetComponent(out EnemyCombatAI combatAI))
                combatAI.SetTarget(target);
            
            if(TryGetComponent(out EnemyNavigationAI navigationAI))
                navigationAI.SetDestination(destination);
        }

        private void OnCharacterDead() => _despawner.Despawn(this);
    }
}