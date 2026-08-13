using System;
using GameObjects.Components;
using UnityEngine;
using Zenject;

namespace GameObjects.Content
{
    [Serializable]
    public class MonkeyCommonComponentInstaller : Installer
    {
        [SerializeField]
        private Rigidbody2D _rigidbody;
        
        [SerializeField]
        private Transform _transform;
        
        [SerializeField]
        private CollisionComponent _collisionComponent;

        [SerializeField]
        private TargetComponent _targetComponent;
        
        [SerializeField]
        private GroundedComponent.Settings _groundedSettings;
        
        [SerializeField]
        private DetectTargetComponent.Settings _detectTargetSettings;
        
        public override void InstallBindings()
        {
            Container.Bind<TargetComponent>().FromInstance(_targetComponent).AsSingle();
            
            Container.Bind<Rigidbody2D>()
                .FromInstance(_rigidbody)
                .AsSingle();
            
            Container.Bind<Transform>()
                .FromInstance(_transform)
                .AsSingle();
            
            Container.Bind<CollisionComponent>()
                .FromInstance(_collisionComponent)
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<GroundedComponent>()
                .AsSingle()
                .WithArguments(_groundedSettings);
            
            Container.BindInterfacesAndSelfTo<DetectTargetComponent>()
                .AsSingle()
                .WithArguments(_detectTargetSettings);
            
            Container.Bind<LookComponent>()
                .AsSingle();
        }
    }
}