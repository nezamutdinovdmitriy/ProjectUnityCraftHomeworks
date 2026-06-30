using System;
using UnityEngine;
using Zenject;

namespace Game
{
    [Serializable]
    public class CharacterCommonComponentsInstaller : Installer
    {
        [SerializeField]
        private Rigidbody2D _rigidbody;

        [SerializeField]
        private Transform _transform;
        
        [SerializeField]
        private GroundedComponent.Settings _groundedSettings;
        
        public override void InstallBindings()
        {
            Container.Bind<Transform>()
                .FromInstance(_transform)
                .AsSingle();

            Container.Bind<Rigidbody2D>().FromInstance(_rigidbody);
            
            Container.Bind<LookComponent>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<GroundedComponent>()
                .AsSingle()
                .WithArguments(_groundedSettings);
        }
    }
}