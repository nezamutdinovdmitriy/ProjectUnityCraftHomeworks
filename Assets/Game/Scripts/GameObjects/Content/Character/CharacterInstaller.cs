using UnityEngine;
using Zenject;

namespace Game
{
    public class CharacterInstaller : MonoInstaller
    {
        [SerializeField]
        private MoveTransformComponent.Settings _moveRequestSettings;

        [SerializeField]
        private JumpRequestComponent.Settings _jumpRequestSettings;

        [SerializeField]
        private DeathRequestComponent.Settings _deathRequestSettings;

        [SerializeField]
        private GroundedComponent.Settings _groundedSettings;

        [SerializeField]
        private float _jumpForce;
        
        [SerializeField]
        private Rigidbody2D _rigidbody;

        [SerializeField]
        private Transform _transform;
        
        [SerializeField]
        private float _maxHealth;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Character>().AsSingle().NonLazy();

            Container.Bind<Transform>().FromInstance(_transform).AsSingle();
            
            Container.BindInterfacesAndSelfTo<MoveRequestComponent>().AsSingle();
            
            Container.Bind<MoveTransformComponent>().AsSingle().WithArguments(_moveRequestSettings);
            
            Container.Bind<LookComponent>().AsSingle();
            
            Container.Bind<Rigidbody2D>().FromInstance(_rigidbody);

            Container.BindInterfacesAndSelfTo<JumpRequestComponent>().AsSingle().WithArguments(_jumpRequestSettings);

            Container.Bind<JumpRigidbodyComponent>().AsSingle().WithArguments(_jumpForce);

            Container.Bind<HealthComponent>().AsSingle().WithArguments(_maxHealth);

            Container.BindInterfacesAndSelfTo<DeathRequestComponent>().AsSingle().WithArguments(_deathRequestSettings);

            Container.BindInterfacesAndSelfTo<GroundedComponent>().AsSingle().WithArguments(_groundedSettings);
        }
    }
}

// [Header("Attack")] [SerializeField]
// private GameObject _pushAttack;
// private AttackRequestComponent _pushAttackRequestComponent;
//
// [SerializeField]
// private GameObject _tossAttack;
// private AttackRequestComponent _tossAttackRequestComponent;