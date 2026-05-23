using System;
using Modules;
using UnityEngine;
using Zenject;

namespace GameSystems
{
    [Serializable]
    public class CharacterInstaller : Installer
    {
        [SerializeField]
        private Snake _snake;
        
        public override void InstallBindings()
        {
            Container.Bind<ITickable>().To<MovementController>().AsCached();

            Container.Bind<ISnake>().FromInstance(_snake).AsSingle();
        }
    }
}