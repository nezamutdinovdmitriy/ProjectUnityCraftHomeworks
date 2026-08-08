using System;
using Game.Target;
using UnityEngine;
using Zenject;

namespace Game
{
    [Serializable]
    public class SnakeMovementInstaller : Installer
    {
        [SerializeField]
        private MoveTransformComponent.Settings _moveTransformSettings;

        [SerializeField]
        private DetectTargetComponent.Settings _detectTargetSettings;
        
        [SerializeField]
        private FollowTargetComponent.Settings _followTargetSettings;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MoveRequestComponent>()
                .AsSingle();

            Container.Bind<MoveTransformComponent>()
                .AsSingle()
                .WithArguments(_moveTransformSettings);
            
            Container.BindInterfacesAndSelfTo<DetectTargetComponent>()
                .AsSingle()
                .WithArguments(_detectTargetSettings);
            
            Container.BindInterfacesAndSelfTo<FollowTargetComponent>()
                .AsSingle()
                .WithArguments(_followTargetSettings);
        }
    }
}