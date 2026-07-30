using Atomic.Entities;
using Game.GameEntity.Core.Death;
using UnityEngine;

namespace Game.GameEntity.Content.Enemy
{
    public class EnemyInstaller : SceneEntityInstaller<IGameEntity>
    {
        [SerializeField]
        private PositionInstaller _positionInstaller;

        [SerializeField]
        private RotationInstaller _rotationInstaller;

        [SerializeField]
        private MovementInstaller _movementInstaller;

        [SerializeField]
        private RotateInstaller _rotateInstaller;

        [SerializeField]
        private HealthInstaller _healthInstaller;

        [SerializeField]
        private DamageableInstaller _damageableInstaller;

        [SerializeField]
        private DeathInstaller _deathInstaller;

        [SerializeField]
        private WeaponInstaller _weaponInstaller;

        [SerializeField]
        private FireInstaller _fireInstaller;

        public override void Install(IGameEntity entity)
        {
            _positionInstaller.Install(entity);
            _rotationInstaller.Install(entity);
            _movementInstaller.Install(entity);
            _healthInstaller.Install(entity);
            _damageableInstaller.Install(entity);
            _rotateInstaller.Install(entity);
            _deathInstaller.Install(entity);
            _weaponInstaller.Install(entity);

            entity.GetValue(GameEntityAPI.TakeDamageCommand)
                .AddCondition(_ => entity.IsDead() == false)
                .AddAction(damage => entity.GetValue(GameEntityAPI.CurrentHealth).Value -= damage);

            entity.WhenFixedTick((deltaTime) =>
            {
                if (entity.IsDead())
                {
                    entity.GetValue(GameEntityAPI.DeathDelay).Tick(deltaTime);
                    entity.GetValue(GameEntityAPI.DeathCommand).Invoke();
                }
            });

            entity.GetValue(GameEntityAPI.DeathCommand)
                .AddCondition(() => entity.IsDead() && entity.GetValue(GameEntityAPI.DeathDelay).IsCompleted())
                .AddAction(() =>
                {
                    entity.GetValue(GameEntityAPI.DeathDelay).ResetTime();
                    Destroy(gameObject);
                });
        }
    }
}