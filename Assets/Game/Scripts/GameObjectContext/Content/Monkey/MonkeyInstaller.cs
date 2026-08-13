using UnityEngine;
using Zenject;
using Game;

namespace GameObjects.Content
{
    public class MonkeyInstaller : MonoInstaller
    {
        [SerializeField]
        private int _contactDamage;
        
        [SerializeField]
        private MonkeyAttackInstaller _attackInstaller;
        
        [SerializeField]
        private MonkeyCommonComponentInstaller _commonComponentInstaller;

        [SerializeField]
        private MonkeyLifeCycleInstaller _lifeCycleInstaller;

        [SerializeField]
        private MonkeyMovementInstaller _movementInstaller;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Monkey>()
                .AsSingle()
                .WithArguments(_contactDamage)
                .NonLazy();
            
            Container.Install(_commonComponentInstaller);
            Container.Install(_lifeCycleInstaller);
            Container.Install(_movementInstaller);
            Container.Install(_attackInstaller);
        }
    }
}