using System;
using Zenject;

namespace Game.Scripts.Domain
{
    [Serializable]
    public class SaveSystemInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SaveManager>().AsSingle();
        }
    }
}