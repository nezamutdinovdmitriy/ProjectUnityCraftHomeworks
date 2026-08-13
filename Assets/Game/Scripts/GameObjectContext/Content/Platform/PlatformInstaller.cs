using GameObjects.Components;
using UnityEngine;
using Zenject;

namespace GameObjects.Content
{
    public class PlatformInstaller : MonoInstaller
    {
        [SerializeField]
        private MoveTransformComponent.Settings _moveRequestSettings;

        [SerializeField]
        private PatrolComponent.Settings _patrolComponentSettings;
        
        [SerializeField]
        private Transform _transform;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MoveRequestComponent>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<Platform>().AsSingle().NonLazy();
            
            Container.Bind<Transform>().FromInstance(_transform).AsSingle();
            
            Container.Bind<MoveTransformComponent>().AsSingle().WithArguments(_moveRequestSettings);

            Container.BindInterfacesAndSelfTo<PatrolComponent>().AsSingle().WithArguments(_patrolComponentSettings);
        }
    }
}