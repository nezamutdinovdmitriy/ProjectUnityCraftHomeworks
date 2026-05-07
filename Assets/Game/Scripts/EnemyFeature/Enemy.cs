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

        public void SetDespawner(IEnemyDespawner despawner) => _despawner = despawner;

        private void OnCharacterDead() => _despawner.Despawn(this);
    }
}