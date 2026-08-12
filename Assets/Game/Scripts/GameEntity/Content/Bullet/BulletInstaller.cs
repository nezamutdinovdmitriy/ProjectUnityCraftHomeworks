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
            entity.WhenFixedTick(deltaTime =>
            {
                Quaternion bulletRotation = entity.GetValue(GameEntityAPI.Rotation).Value;
                float movementSpeed = entity.GetValue(GameEntityAPI.MovementSpeed).Value;
                
                entity.MoveStep(bulletRotation * Vector3.forward, movementSpeed, deltaTime);
            });
        }
    }
}