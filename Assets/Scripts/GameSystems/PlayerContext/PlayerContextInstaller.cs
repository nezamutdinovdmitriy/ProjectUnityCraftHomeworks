using Modules;
using UnityEngine;
using Zenject;

namespace GameSystems.PlayerContext
{
    public class PlayerContextInstaller : MonoInstaller
    {
        [SerializeField]
        private Snake _snake;

        public override void InstallBindings()
        {
            Container.Bind<IInputProvider>().To<KeyboardInputProvider>().AsSingle();
            
            Container.Bind<ITickable>().To<SnakeMovementController>().AsCached();

            Container.Bind<ISnake>().FromInstance(_snake).AsSingle();

            Container.BindInterfacesTo<SnakeExpandController>().AsSingle();

            Container.BindInterfacesTo<SnakeSpeedController>().AsSingle();
        }
    }
}