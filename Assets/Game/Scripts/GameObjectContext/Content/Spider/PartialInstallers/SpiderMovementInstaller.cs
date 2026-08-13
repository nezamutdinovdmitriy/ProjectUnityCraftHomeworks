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
        private PatrolComponent.Settings _patrolComponentSettings;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MoveRequestComponent>().AsSingle();

            Container.Bind<MoveTransformComponent>()
                .AsSingle()
                .WithArguments(_moveTransformSettings);
            
            Container.BindInterfacesAndSelfTo<PatrolComponent>()
                .AsSingle()
                .WithArguments(_patrolComponentSettings);
        }
    }
}