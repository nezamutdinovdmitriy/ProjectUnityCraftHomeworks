using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntity.Core.Aim;
using Game.GameEntity.Core.Fire;
using Game.Weapon;
using UnityEngine;

namespace Game.GameEntity.Content.Character
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

        public override void Install(IGameEntity entity)
        {
            _positionInstaller.Install(entity);
            _rotationInstaller.Install(entity);
            _movementInstaller.Install(entity);
            _rotateInstaller.Install(entity);
            _healthInstaller.Install(entity);
            _fireInstaller.Install(entity);
            _weaponInstaller.Install(entity);
            _aimInstaller.Install(entity);

            SetupMovementCommand(entity);
            SetupRotateCommand(entity);
            SetupFireCommand(entity);
            
            entity.AddBehaviour(new CharacterInputController());
        }

        private void SetupFireCommand(IGameEntity entity)
        {
            IReactiveVariable<IWeaponEntity> weapon = entity.GetValue(GameEntityAPI.Weapon);
            
            entity.GetValue(GameEntityAPI.FireCommand)
                .AddCondition(() 
                    => entity.IsDead() == false && entity.GetValue(GameEntityAPI.AimCooldown).IsCompleted())
                .AddAction(() => weapon.Value.GetValue(WeaponEntityAPI.FireCommand).Invoke());
        }
        
        private void SetupRotateCommand(IGameEntity entity)
        {
            entity.GetValue(GameEntityAPI.RotateCommand)
                .AddCondition(args => entity.IsDead() == false && args.Direction != Vector3.zero)
                .AddAction(args => entity.RotateStep(args.Direction, args.Speed, args.DeltaTime));
        }

        private void SetupMovementCommand(IGameEntity entity)
        {
            entity.GetValue(GameEntityAPI.MovementCommand)
                .AddCondition(args => entity.IsDead() == false && args.Direction != Vector3.zero)
                .AddAction(args => entity.MoveStep(args.Direction, args.Speed, args.DeltaTime))
                .AddAction(args => entity.RotateStep(
                    args.Direction,
                    entity.GetValue(GameEntityAPI.RotateSpeed).Value,
                    args.DeltaTime));
        }
    }
}