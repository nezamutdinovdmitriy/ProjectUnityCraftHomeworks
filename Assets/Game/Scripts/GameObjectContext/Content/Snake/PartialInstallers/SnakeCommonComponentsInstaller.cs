using System;
using GameObjects.Components;
using UnityEngine;
using Zenject;

namespace GameObjects.Content
{
    [Serializable]
    public class SnakeCommonComponentsInstaller : Installer
    {
        [SerializeField]
        private Transform _transform;

        [SerializeField]
        private TargetComponent _targetComponent;
        
        [SerializeField]
        private CollisionComponent _collisionComponent;

        [SerializeField]
        private GroundedComponent.Settings _groundedSettings;
        
        private LookComponent _lookComponent;
        
        public override void InstallBindings()
        {
            Container.Bind<TargetComponent>().FromInstance(_targetComponent).AsSingle();
            
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