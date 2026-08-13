using System;
using GameObjects.Components;
using UnityEngine;
using Zenject;

namespace GameObjects.Content
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