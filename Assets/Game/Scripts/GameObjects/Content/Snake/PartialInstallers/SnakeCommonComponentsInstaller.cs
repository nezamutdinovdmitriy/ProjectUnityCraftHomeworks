using System;
using UnityEngine;
using Zenject;

namespace Game
{
    [Serializable]
    public class SnakeCommonComponentsInstaller : Installer
    {
        [SerializeField]
        private Transform _transform;
        
        [SerializeField]
        private CollisionComponent _collisionComponent;

        [SerializeField]
        private GroundedComponent.Settings _groundedSettings;
        
        private LookComponent _lookComponent;
        
        public override void InstallBindings()
        {
            Container.Bind<Transform>()
                .FromInstance(_transform)
                .AsSingle();
            
            Container.Bind<CollisionComponent>()
                .FromInstance(_collisionComponent)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<GroundedComponent>()
                .AsSingle()
                .WithArguments(_groundedSettings);
            
            Container.Bind<LookComponent>().AsSingle();
        }
    }
}