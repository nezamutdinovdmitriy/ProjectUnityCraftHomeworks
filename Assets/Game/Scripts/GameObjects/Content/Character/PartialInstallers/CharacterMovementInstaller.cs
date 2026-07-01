using System;
using UnityEngine;
using Zenject;

namespace Game
{
    [Serializable]
    public class CharacterMovementInstaller : Installer
    {
        [SerializeField]
        private MoveTransformComponent.Settings _moveRequestSettings;
        
        [SerializeField]
        private JumpRequestComponent.Settings _jumpRequestSettings;

        [SerializeField]
        private ExtraGravityComponent.Settings _extraGravitySettings;
        
        [SerializeField]
        private float _jumpForce;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MoveRequestComponent>()
                .AsSingle();
            
            Container.Bind<MoveTransformComponent>()
                .AsSingle()
                .WithArguments(_moveRequestSettings);
            
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