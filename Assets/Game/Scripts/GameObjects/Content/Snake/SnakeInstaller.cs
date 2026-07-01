using UnityEngine;
using Zenject;

namespace Game
{
    public class SnakeInstaller : MonoInstaller
    {
        [SerializeField]
        private int _contactDamage;
        
        [SerializeField]
        private SnakeCommonComponentsInstaller _commonComponentsInstaller;
        
        [SerializeField]
        private SnakeMovementInstaller _movementInstaller;

        [SerializeField]
        private SnakeLifeCycleInstaller _lifeCycleInstaller;

        [SerializeField]
        private SnakeAttackInstaller _attackInstaller;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Snake>()
                .AsSingle()
                .WithArguments(_contactDamage)
                .NonLazy();

            Container.Install(_commonComponentsInstaller);
            Container.Install(_movementInstaller);
            Container.Install(_lifeCycleInstaller);
            Container.Install(_attackInstaller);
        }
    }
}