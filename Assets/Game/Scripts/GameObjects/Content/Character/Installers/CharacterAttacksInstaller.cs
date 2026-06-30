using System;
using UnityEngine;
using Zenject;

namespace Game
{
    [Serializable]
    public class CharacterAttacksInstaller : Installer
    {
        [SerializeField]
        private AttackRequestComponent.Settings _pushAttackRequestSettings;
        
        [SerializeField]
        private AttackRequestComponent.Settings _tossAttackRequestSettings;

        [SerializeField]
        private ForceAttackComponent.Settings _pushAttackSettings;

        [SerializeField]
        private ForceAttackComponent.Settings _tossAttackSettings;
        
        public override void InstallBindings()
        {
            PushAttackBind();
            TossAttackBind();
        }

        private void TossAttackBind()
        {
            Container.Bind<AttackRequestComponent>()
                .WithId(AttackType.Toss)
                .AsCached()
                .WithArguments(_tossAttackRequestSettings);
            
            Container.Bind<IFixedTickable>()
                .To<AttackRequestComponent>()
                .FromResolve(AttackType.Toss);
    
            Container.Bind<ForceAttackComponent>()
                .WithId(AttackType.Toss)
                .AsCached()
                .WithArguments(_tossAttackSettings);
        }

        private void PushAttackBind()
        {
            Container.Bind<AttackRequestComponent>()
                .WithId(AttackType.Push)
                .AsCached()
                .WithArguments(_pushAttackRequestSettings);
            
            Container.Bind<IFixedTickable>()
                .To<AttackRequestComponent>()
                .FromResolve(AttackType.Push);
            
            Container.Bind<ForceAttackComponent>()
                .WithId(AttackType.Push)
                .AsCached()
                .WithArguments(_pushAttackSettings);
        }
    }
}