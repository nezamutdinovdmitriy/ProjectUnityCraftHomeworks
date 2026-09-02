using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntities
{
    public class BulletInstaller : SceneEntityInstaller<IGameEntity>
    {
        private readonly DisposableComposite _disposables = new();

        [Space] [Header("Installers")] [SerializeField]
        private PositionInstaller _positionInstaller;

        [SerializeField]
        private RotationInstaller _rotationInstaller;

        [SerializeField]
        private DamageInstaller _damageInstaller;

        [SerializeField]
        private LifeTimeInstaller _lifetimeInstaller;

        [SerializeField]
        private float _moveSpeed;
        
        public override void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.Owner, new Variable<IGameEntity>());

            _positionInstaller.Install(entity);
            _rotationInstaller.Install(entity);
            _damageInstaller.Install(entity);
            _lifetimeInstaller.Install(entity);

            entity.WhenFixedTick( deltaTime 
                => entity.MoveStepForward(_moveSpeed, deltaTime))
                .AddTo(_disposables);

            entity.GetValue(GameEntityAPI.DestroyAction).Add(() 
                => GameContext.Instance.DestroyBullet((GameEntity) entity));

            entity.AddBehaviour(new BulletDamageBehaviour(GameContext.Instance));
        }

        public override void Uninstall(IGameEntity entity) 
            => _disposables.Dispose();
    }
}