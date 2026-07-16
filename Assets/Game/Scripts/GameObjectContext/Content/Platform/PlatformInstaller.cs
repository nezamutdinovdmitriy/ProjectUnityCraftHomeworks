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
        private PatrolComponent.Settings _pointProviderSettings;

        [SerializeField]
        private FollowTargetComponent.Settings _followTargetSettings;

        [SerializeField]
        private TargetComponent _targetComponent;
        
        [SerializeField]
        private Transform _transform;
        
        public override void InstallBindings()
        {
            Container.Bind<TargetComponent>().FromInstance(_targetComponent).AsSingle();
            
            Container.BindInterfacesAndSelfTo<Platform>().AsSingle().NonLazy();
            
            Container.Bind<Transform>().FromInstance(_transform).AsSingle();
            
            Container.Bind<MoveTransformComponent>().AsSingle().WithArguments(_moveRequestSettings);

            Container.BindInterfacesAndSelfTo<PatrolComponent>().AsSingle().WithArguments(_pointProviderSettings);

            Container.Bind<FollowTargetComponent>().AsSingle().WithArguments(_followTargetSettings);
        }
    }
}