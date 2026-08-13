using System;
using GameObjects.Components;
using UnityEngine;
using Zenject;

namespace GameObjects.Content
{
    [Serializable]
    public class SpiderCommonComponentInstaller : Installer
    {
        [SerializeField]
        private Transform _transform;
        
        [SerializeField]
        private CollisionComponent _collisionComponent;

        [SerializeField]
        private GroundedComponent.Settings _groundedComponentSettings;
        
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
                .WithArguments(_groundedComponentSettings);
        }
    }
}