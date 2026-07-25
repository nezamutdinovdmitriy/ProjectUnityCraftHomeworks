using Atomic.Entities;
using Game.GameEntity.Core.Aim;
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
        
        // [SerializeField]
        // private FireInstaller _fireInstaller;

        public override void Install(IGameEntity entity)
        {
            _positionInstaller.Install(entity);
            _rotationInstaller.Install(entity);
            _movementInstaller.Install(entity);
            _rotateInstaller.Install(entity);
            _healthInstaller.Install(entity);
            //_fireInstaller.Install(entity);
            _aimInstaller.Install(entity);

            entity.GetValue(GameEntityAPI.MovementCommand)
                .AddCondition(args => entity.IsDead() == false && args.Direction != Vector3.zero)
                .AddAction(args => entity.MoveStep(args.Direction, args.Speed, args.DeltaTime))
                .AddAction(args => entity.RotateStep(
                    args.Direction,
                    entity.GetValue(GameEntityAPI.RotateSpeed).Value,
                    args.DeltaTime));

            entity.GetValue(GameEntityAPI.RotateCommand)
                .AddCondition(args => entity.IsDead() == false && args.Direction != Vector3.zero)
                .AddAction(args => entity.RotateStep(args.Direction, args.Speed, args.DeltaTime));

            // entity.GetValue(GameEntityAPI.FireCommand)
            //     .AddCondition(() 
            //         => entity.IsDead() == false && entity.GetValue(GameEntityAPI.AimCooldown).IsCompleted())
            //     .AddAction(() => Debug.Log("FIRED"));

            entity.AddBehaviour(new CharacterInputController());
        }

        public override void Uninstall(IGameEntity entity)
        {
            _aimInstaller.Uninstall();
        }
    }
}