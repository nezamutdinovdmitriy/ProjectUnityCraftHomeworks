using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntities
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

            MovementCommandSetup(entity);
            RotateCommandSetup(entity);
            FireCommandSetup(entity);
            TakeDamageCommandSetup(entity);
            
            entity.GetValue(GameEntityAPI.DeathAction).Add(() =>
            {
                entity.GetValue(GameEntityAPI.DeathDelay).ResetTime();
                Destroy(gameObject);
            });
            
            entity.AddBehaviour(new FollowTargetBehaviour(_stoppingDistance));
            entity.AddBehaviour(new AttackTargetBehaviour());
        }

        private static void RotateCommandSetup(IGameEntity entity)
        {
            entity.GetValue(GameEntityAPI.RotateCommand)
                .AddCondition(args 
                    => entity.IsDead() == false 
                       && entity.HasDirection(args.Direction))
                .AddAction(args 
                    => entity.RotateStep(args.Direction, args.Speed, args.DeltaTime));
        }

        private static void MovementCommandSetup(IGameEntity entity)
        {
            entity.GetValue(GameEntityAPI.MovementCommand)
                .AddCondition(args 
                    => entity.IsDead() == false 
                       && entity.HasDirection(args.Direction))
                .AddAction(args 
                    => entity.MoveStep(args.Direction, args.Speed, args.DeltaTime));
        }

        private static void FireCommandSetup(IGameEntity entity)
        {
            entity.GetValue(GameEntityAPI.FireCommand)
                .AddCondition(() =>
                    entity.IsDead() == false
                    && entity.HasWeapon())
                .AddAction(entity.InvokeFireRequest);
        }

        private void TakeDamageCommandSetup(IGameEntity entity)
        {
            entity.GetValue(GameEntityAPI.TakeDamageCommand)
                .AddCondition(_ => entity.IsDead() == false)
                .AddAction(entity.HealthReduce);
        }
    }
}