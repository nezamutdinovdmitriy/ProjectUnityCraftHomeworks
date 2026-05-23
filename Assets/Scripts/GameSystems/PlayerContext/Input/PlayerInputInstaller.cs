using System;
using Zenject;

namespace GameSystems
{
    [Serializable]
    public class PlayerInputInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<IInputProvider>().To<DesktopInputProvider>().AsSingle();
        }
    }
}