using Zenject;

namespace Game.Scripts.Extensions
{
    public static class ZenjectExtension
    {
        public static void Install(DiContainer container, Installer installer)
        {
            container.Inject(installer);
            installer.InstallBindings();
        }
    }
}