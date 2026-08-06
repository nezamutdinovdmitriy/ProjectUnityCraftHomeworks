using Atomic.Elements;
using Atomic.Entities;
using Game.Bullets;
using Game.GameEntity.Core.LifeTime;
using UnityEngine;

namespace Game.GameEntity
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

        [SerializeField]
        private LifeTimeInstaller _lifetimeInstaller;

        public override void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.Owner, new Variable<IGameEntity>());

            _positionInstaller.Install(entity);
            _rotationInstaller.Install(entity);
            _movementInstaller.Install(entity);
            _damageInstaller.Install(entity);
            _lifetimeInstaller.Install(entity);

            SetupMovementBehaviour(entity);
            SetupLifetimeEndBehaviour(entity);

            entity.AddBehaviour(new CollisionDamageBehaviour(GameContext.Instance));
        }

        public override void Uninstall(IGameEntity entity)
        {
            _lifetimeInstaller.Dispose();
            entity.Clear();
        }

        private static void SetupLifetimeEndBehaviour(IGameEntity entity)
        {
            entity.GetValue(GameEntityAPI.LifetimeEndCommand)
                .AddAction(() => GameContext.Instance.DestroyBullet((GameEntity) entity));
        }

        private static void SetupMovementBehaviour(IGameEntity entity)
        {
            IRequest<Vector3> movementRequest = entity.GetValue(GameEntityAPI.MovementRequest);

            entity.WhenFixedTick(_ =>
            {
                Quaternion rotation = entity.GetValue(GameEntityAPI.Rotation).Value;
                movementRequest.Invoke(rotation * Vector3.forward);
            });

            entity.GetValue(GameEntityAPI.MovementCommand)
                .AddCondition(_ => true)
                .AddAction(args => entity.MoveStep(args.Direction, args.Speed, args.DeltaTime));
        }
    }
}