using System;
using GameObjects.Components;
using UnityEngine;
using Zenject;

namespace GameObjects.Content
{
    [Serializable]
    public class MonkeyMovementInstaller : Installer
    {
        [SerializeField]
        private float _jumpForce;
        
        [SerializeField]
        private JumpRequestComponent.Settings _jumpRequestSettings;
        
        [SerializeField]
        private ExtraGravityComponent.Settings _extraGravitySettings;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<JumpRequestComponent>()
                .AsSingle()
                .WithArguments(_jumpRequestSettings);
            
            Container.Bind<JumpRigidbodyComponent>()
                .AsSingle()
                .WithArguments(_jumpForce);
            
            Container.BindInterfacesAndSelfTo<ExtraGravityComponent>()
                .AsSingle()
                .WithArguments(_extraGravitySettings);
        }
    }
}