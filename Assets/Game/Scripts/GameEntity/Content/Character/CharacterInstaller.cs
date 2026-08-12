using Atomic.Elements;
using Atomic.Entities;
using Game.Weapon;
using UnityEngine;

namespace Game.GameEntity
{
    public class CharacterInstaller : SceneEntityInstaller<IGameEntity>
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
        private AimInstaller _aimInstaller;
        
        [SerializeField]
        private WeaponInstaller _weaponInstaller;

        [SerializeField]
        private FireInstaller _fireInstaller;

        [SerializeField]
        private InteractorInstaller _interactorInstaller;

        [SerializeField]
        private DamageableInstaller _damageableInstaller;

        public override void Install(IGameEntity entity)
        {
            entity.AddTag(GameEntityAPI.CharacterTag);
            
            _positionInstaller.Install(entity);
            _rotationInstaller.Install(entity);
            _movementInstaller.Install(entity);
            _rotateInstaller.Install(entity);
            _healthInstaller.Install(entity);
            _fireInstaller.Install(entity);
            _weaponInstaller.Install(entity);
            _aimInstaller.Install(entity);
            _interactorInstaller.Install(entity);
            _damageableInstaller.Install(entity);

            SetupMovementCommand(entity);
            SetupRotateCommand(entity);
            SetupFireCommand(entity);

            entity.GetValue(GameEntityAPI.TakeDamageCommand)
                .AddCondition(_ => entity.IsDead() == false)
                .AddAction(entity.HealthReduce);
        }

        private void SetupFireCommand(IGameEntity entity)
        {
            entity.GetValue(GameEntityAPI.FireCommand)
                .AddCondition(() 
                    => entity.IsDead() == false 
                       && entity.HasWeapon()
                       && entity.IsAimDelayCompleted())
                .AddAction(entity.InvokeFireRequest);
        }
        
        private void SetupRotateCommand(IGameEntity entity)
        {
            entity.GetValue(GameEntityAPI.RotateCommand)
                .AddCondition(args 
                    => entity.IsDead() == false 
                       && entity.HasDirection(args.Direction))
                .AddAction(args 
                    => entity.RotateStep(args.Direction, args.Speed, args.DeltaTime));
        }

        private void SetupMovementCommand(IGameEntity entity)
        {
            entity.GetValue(GameEntityAPI.MovementCommand)
                .AddCondition(args => entity.IsDead() == false && entity.HasDirection(args.Direction))
                .AddAction(args => entity.MoveStep(args.Direction, args.Speed, args.DeltaTime));
        }
    }
}