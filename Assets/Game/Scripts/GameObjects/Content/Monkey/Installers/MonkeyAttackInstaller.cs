using System;
using UnityEngine;
using Zenject;

namespace Game
{
    [Serializable]
    public class MonkeyAttackInstaller : Installer
    {
        [SerializeField]
        private AttackRequestComponent.Settings _pushAttackRequestSettings;
        
        [SerializeField]
        private ForceAttackComponent.Settings _pushAttackSettings;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AttackRequestComponent>()
                .AsSingle()
                .WithArguments(_pushAttackRequestSettings);

            Container.Bind<ForceAttackComponent>().AsSingle().WithArguments(_pushAttackSettings);
        }
    }
}