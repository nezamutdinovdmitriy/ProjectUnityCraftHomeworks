using Atomic.Entities;
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
        private float _rotationSpeed;

        [SerializeField]
        private HealthInstaller _healthInstaller;
        
        public override void Install(IGameEntity entity)
        {
            _positionInstaller.Install(entity);
            _rotationInstaller.Install(entity);
            _movementInstaller.Install(entity);
            _rotateInstaller.Install(entity);
            _healthInstaller.Install(entity);

            entity.GetValue(GameEntityAPI.MovementCommand)
                .AddCondition(args => entity.IsDead() == false && args.Direction != Vector3.zero)
                .AddAction(args => entity.MoveStep(args.Direction, args.Speed, args.DeltaTime))
                .AddAction(args => entity.RotateStep(args.Direction, args.DeltaTime));
            
            entity.AddBehaviour(new CharacterInputController());
        }
    }
}