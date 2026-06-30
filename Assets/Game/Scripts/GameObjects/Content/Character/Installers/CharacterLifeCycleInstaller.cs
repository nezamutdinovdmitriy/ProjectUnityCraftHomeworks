using System;
using UnityEngine;
using Zenject;

namespace Game
{
    [Serializable]
    public class CharacterLifeCycleInstaller : Installer
    {
        [SerializeField]
        private float _maxHealth;
        
        [SerializeField]
        private DeathRequestComponent.Settings _deathRequestSettings;
        
        public override void InstallBindings()
        {
            Container.Bind<HealthComponent>()
                .AsSingle()
                .WithArguments(_maxHealth);
            
            Container.BindInterfacesAndSelfTo<DeathRequestComponent>()
                .AsSingle()
                .WithArguments(_deathRequestSettings);
        }
    }
}