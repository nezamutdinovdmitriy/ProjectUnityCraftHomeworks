using UnityEngine;
using Zenject;

namespace Game
{
    public class SpiderInstaller : MonoInstaller
    {
        [SerializeField]
        private int _contactDamage;

        [SerializeField]
        private SpiderCommonComponentInstaller _commonComponentInstaller;

        [SerializeField]
        private SpiderAttackInstaller _attackInstaller;

        [SerializeField]
        private SpiderLifeCycleInstaller _lifeCycleInstaller;

        [SerializeField]
        private SpiderMovementInstaller _movementInstaller;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Spider>()
                .AsSingle()
                .WithArguments(_contactDamage)
                .NonLazy();

            Container.Install(_commonComponentInstaller);
            Container.Install(_attackInstaller);
            Container.Install(_lifeCycleInstaller);
            Container.Install(_movementInstaller);
        }
    }
}