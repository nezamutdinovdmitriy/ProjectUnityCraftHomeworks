using System;
using Game.Patrol;
using Game.Target;
using UnityEngine;
using Zenject;

namespace Game
{
    [Serializable]
    public class SpiderMovementInstaller : Installer
    {
        [SerializeField]
        private MoveTransformComponent.Settings _moveTransformSettings;
        
        [SerializeField]
        private FollowTargetComponent.Settings _followTargetSettings;

        [SerializeField]
        private PointProviderComponent.Settings _pointProviderSettings;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MoveRequestComponent>().AsSingle();

            Container.Bind<MoveTransformComponent>()
                .AsSingle()
                .WithArguments(_moveTransformSettings);
            
            Container.Bind<FollowTargetComponent>()
                .AsSingle()
                .WithArguments(_followTargetSettings);
            
            Container.BindInterfacesAndSelfTo<PointProviderComponent>()
                .AsSingle()
                .WithArguments(_pointProviderSettings);
        }
    }
}