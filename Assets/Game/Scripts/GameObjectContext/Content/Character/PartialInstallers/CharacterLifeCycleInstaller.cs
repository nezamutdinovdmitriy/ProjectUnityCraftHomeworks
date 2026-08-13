using System;
using GameObjects.Components;
using UnityEngine;
using Zenject;

namespace GameObjects.Content
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