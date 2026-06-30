using Game.Target;
using UnityEngine;
using Zenject;

namespace Game.Scripts.GameObjects.Content.Monkey
{
    public class MonkeyInstaller : MonoInstaller
    {
        [SerializeField]
        private int _damage;

        [SerializeField]
        private Rigidbody2D _rigidbody;
        
        [SerializeField]
        private Transform _transform;
        
        [SerializeField]
        private float _jumpForce;
        
        [SerializeField]
        private JumpRequestComponent.Settings _jumpRequestSettings;
        
        [SerializeField]
        private float _maxHealth;
        
        [SerializeField]
        private DeathRequestComponent.Settings _deathRequestSettings;
        
        [SerializeField]
        private GroundedComponent.Settings _groundedSettings;

        [SerializeField]
        private DetectTargetComponent.Settings _detectTargetSettings;

        [SerializeField]
        private CollisionComponent _collisionComponent;
        
        public override void InstallBindings()
        {
            Container.Bind<Transform>().FromInstance(_transform).AsSingle();
            
            Container.BindInterfacesAndSelfTo<Monkey>().AsSingle().WithArguments(_damage).NonLazy();
            
            Container.BindInterfacesAndSelfTo<JumpRequestComponent>().AsSingle().WithArguments(_jumpRequestSettings);

            Container.Bind<Rigidbody2D>().FromInstance(_rigidbody);
            
            Container.Bind<JumpRigidbodyComponent>().AsSingle().WithArguments(_jumpForce);
            
            Container.Bind<HealthComponent>().AsSingle().WithArguments(_maxHealth);

            Container.BindInterfacesAndSelfTo<DeathRequestComponent>().AsSingle().WithArguments(_deathRequestSettings);
            
            Container.BindInterfacesAndSelfTo<GroundedComponent>().AsSingle().WithArguments(_groundedSettings);
            
            Container.Bind<LookComponent>().AsSingle();

            Container.BindInterfacesAndSelfTo<DetectTargetComponent>().AsSingle().WithArguments(_detectTargetSettings);

            Container.Bind<CollisionComponent>().FromInstance(_collisionComponent).AsSingle();
        }
    }
}