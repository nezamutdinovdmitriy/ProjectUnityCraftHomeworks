using System;
using UnityEngine;
using Zenject;

namespace Game
{
    [Serializable]
    public class SnakeAttackInstaller : Installer
    {
        [SerializeField]
        private AttackRequestComponent.Settings _attackRequestSettings;
        
        [SerializeField]
        private ForceAttackComponent.Settings _forceAttackSettings;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AttackRequestComponent>()
                .AsSingle()
                .WithArguments(_attackRequestSettings);

            Container.Bind<ForceAttackComponent>()
                .AsSingle()
                .WithArguments(_forceAttackSettings);
        }
    }
}