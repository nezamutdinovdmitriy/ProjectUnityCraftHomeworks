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
            Container.Bind<IInputProvider>().To<DesktopInputProvider>().AsSingle();
            
            Container.Bind<ITickable>().To<MovementController>().AsCached();

            Container.Bind<ISnake>().FromInstance(_snake).AsSingle();

            Container.BindInterfacesTo<SnakeExpandController>().AsSingle();
        }
    }
}