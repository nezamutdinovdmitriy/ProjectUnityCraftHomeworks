using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntity.Core.Death;
using Game.GameEntity.Core.Target;
using Game.Weapon;
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

        [SerializeField]
        private TargetInstaller _targetInstaller;

        [SerializeField]
        private float _stoppingDistance;

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
            _targetInstaller.Install(entity);
            _fireInstaller.Install(entity);

            entity.WhenFixedTick((deltaTime) =>
            {
                if (entity.IsDead())
                {
                    entity.GetValue(GameEntityAPI.DeathDelay).Tick(deltaTime);
                    entity.GetValue(GameEntityAPI.DeathCommand).Invoke();
                }
            });

            entity.WhenFixedTick(_ =>
            {
                if (entity.TryGetValue(GameEntityAPI.Target, out IVariable<IGameEntity> target)
                    && target.Value != null
                    && target.Value.IsDead() == false)
                {
                    Vector3 targetPosition = target.Value.GetValue(GameEntityAPI.Position).Value;

                    Vector3 selfPosition = entity.GetValue(GameEntityAPI.Position).Value;
                    Vector3 moveDirection = (targetPosition - selfPosition).normalized;

                    bool isReached = (targetPosition - selfPosition).magnitude <= _stoppingDistance;

                    if (isReached == false)
                        entity.GetValue(GameEntityAPI.MovementRequest).Invoke(moveDirection);

                    entity.GetValue(GameEntityAPI.RotateRequest).Invoke(moveDirection);

                    if (isReached)
                        entity.GetValue(GameEntityAPI.FireRequest).Invoke();
                }
            });

            entity.GetValue(GameEntityAPI.MovementCommand)
                .AddCondition(args => entity.IsDead() == false && args.Direction != Vector3.zero)
                .AddAction(args => entity.MoveStep(args.Direction, args.Speed, args.DeltaTime));

            entity.GetValue(GameEntityAPI.RotateCommand)
                .AddCondition(args => entity.IsDead() == false && args.Direction != Vector3.zero)
                .AddAction(args => entity.RotateStep(args.Direction, args.Speed, args.DeltaTime));

            FireCommandSetup(entity);
            TakeDamageCommandSetup(entity);
            DeathCommandSetup(entity);
        }

        private static void FireCommandSetup(IGameEntity entity)
        {
            IWeaponEntity weapon = entity.GetValue(GameEntityAPI.Weapon).Value;
            
            entity.GetValue(GameEntityAPI.FireCommand)
                .AddCondition(() =>
                    entity.IsDead() == false
                    && weapon != null)
                .AddAction(() =>
                    weapon.GetValue(WeaponEntityAPI.FireRequest).Invoke());
        }

        private void DeathCommandSetup(IGameEntity entity)
        {
            entity.GetValue(GameEntityAPI.DeathCommand)
                .AddCondition(() => entity.IsDead() && entity.GetValue(GameEntityAPI.DeathDelay).IsCompleted())
                .AddAction(() =>
                {
                    entity.GetValue(GameEntityAPI.DeathDelay).ResetTime();
                    Destroy(gameObject);
                });
        }

        private void TakeDamageCommandSetup(IGameEntity entity)
        {
            entity.GetValue(GameEntityAPI.TakeDamageCommand)
                .AddCondition(_ => entity.IsDead() == false)
                .AddAction(damage => entity.GetValue(GameEntityAPI.CurrentHealth).Value -= damage);
        }
    }
}