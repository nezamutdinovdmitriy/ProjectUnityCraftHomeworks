using System;
using GameObjects.Components;
using UnityEngine;
using Zenject;

namespace GameObjects.Content
{
    [Serializable]
    public class SpiderMovementInstaller : Installer
    {
        [SerializeField]
        private MoveTransformComponent.Settings _moveTransformSettings;
        
        [SerializeField]
        private FollowTargetComponent.Settings _followTargetSettings;

        [SerializeField]
        private PatrolComponent.Settings _pointProviderSettings;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MoveRequestComponent>().AsSingle();

            Container.Bind<MoveTransformComponent>()
                .AsSingle()
                .WithArguments(_moveTransformSettings);
            
            Container.BindInterfacesAndSelfTo<FollowTargetComponent>()
                .AsSingle()
                .WithArguments(_followTargetSettings);
            
            Container.BindInterfacesAndSelfTo<PatrolComponent>()
                .AsSingle()
                .WithArguments(_pointProviderSettings);
        }
    }
}