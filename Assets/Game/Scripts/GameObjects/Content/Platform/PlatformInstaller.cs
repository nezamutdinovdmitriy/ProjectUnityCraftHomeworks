using Game.Patrol;
using Game.Target;
using UnityEngine;
using Zenject;

namespace Game
{
    public class PlatformInstaller : MonoInstaller
    {
        [SerializeField]
        private MoveTransformComponent.Settings _moveRequestSettings;

        [SerializeField]
        private PointProviderComponent.Settings _pointProviderSettings;

        [SerializeField]
        private FollowTargetComponent.Settings _followTargetSettings;

        [SerializeField]
        private GroundedComponent.Settings _groundedSettings;

        [SerializeField]
        private Transform _transform;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Platform>().AsSingle().NonLazy();

            Container.Bind<StandingPlatformComponent>().AsSingle();
            Container.Bind<GroundedComponent>().AsSingle().WithArguments(_groundedSettings);
            
            Container.Bind<Transform>().FromInstance(_transform).AsSingle();
            
            Container.Bind<MoveTransformComponent>().AsSingle().WithArguments(_moveRequestSettings);

            Container.BindInterfacesAndSelfTo<PointProviderComponent>().AsSingle().WithArguments(_pointProviderSettings);

            Container.Bind<FollowTargetComponent>().AsSingle().WithArguments(_followTargetSettings);
        }
    }
}