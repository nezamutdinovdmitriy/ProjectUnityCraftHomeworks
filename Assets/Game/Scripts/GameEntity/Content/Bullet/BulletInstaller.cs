using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntity.Content.Bullet
{
    public class BulletInstaller : SceneEntityInstaller<IGameEntity>
    {
        [Space] [Header("Installers")] [SerializeField]
        private PositionInstaller _positionInstaller;

        [SerializeField]
        private RotationInstaller _rotationInstaller;

        [SerializeField]
        private MovementInstaller _movementInstaller;

        [SerializeField]
        private DamageInstaller _damageInstaller;

        public override void Install(IGameEntity entity)
        {
            _positionInstaller.Install(entity);
            _rotationInstaller.Install(entity);
            _movementInstaller.Install(entity);
            _damageInstaller.Install(entity);
            
            SetupMovementBehaviour(entity);
            
            entity.AddBehaviour(new CollisionDamageBehaviour());
        }

        private static void SetupMovementBehaviour(IGameEntity entity)
        {
            IRequest<Vector3> movementRequest = entity.GetValue(GameEntityAPI.MovementRequest);

            entity.WhenFixedTick(_ => movementRequest.Invoke(Vector3.forward));
            entity.GetValue(GameEntityAPI.MovementCommand)
                .AddCondition(_ => true)
                .AddAction(args => entity.MoveStep(args.Direction, args.Speed, args.DeltaTime));
        }
    }
}