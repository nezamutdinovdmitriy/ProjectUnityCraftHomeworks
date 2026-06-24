using System.Runtime.CompilerServices;
using Zenject;

namespace Game.Scripts.Extensions
{
    public static class ZenjectExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Install(DiContainer container, Installer installer)
        {
            container.Inject(installer);
            installer.InstallBindings();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Install(DiContainer container, Installer installer, params object[] extraArgs)
        {
            container.Inject(installer, extraArgs);
            installer.InstallBindings();
        }
    }
}