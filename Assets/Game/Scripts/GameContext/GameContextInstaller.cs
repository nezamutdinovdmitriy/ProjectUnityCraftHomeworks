using UnityEngine;
using Zenject;
using Game;
using GameObjects.Components;

namespace GameContexts
{
    public class GameContextInstaller : MonoInstaller
    {
        private readonly PlayerInputInstaller _playerInputInstaller = new();

        [SerializeField]
        private Entity _character;
        
        public override void InstallBindings()
        {
            Container.Bind<CharacterProvider>().AsSingle().WithArguments(_character);
            
            Container.Install(_playerInputInstaller);
        }
    }
}