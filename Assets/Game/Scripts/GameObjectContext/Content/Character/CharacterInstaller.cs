using UnityEngine;
using Zenject;

namespace Game
{
    public class CharacterInstaller : MonoInstaller
    {
        [SerializeField]
        private CharacterCommonComponentsInstaller _commonComponentsInstaller;
        
        [SerializeField]
        private CharacterAttacksInstaller _attacksInstaller;

        [SerializeField]
        private CharacterLifeCycleInstaller _lifeCycleInstaller;

        [SerializeField]
        private CharacterMovementInstaller _movementInstaller;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Character>().AsSingle().NonLazy();

            Container.Install(_commonComponentsInstaller);
            Container.Install(_attacksInstaller);
            Container.Install(_lifeCycleInstaller);
            Container.Install(_movementInstaller);
        }
    }
}