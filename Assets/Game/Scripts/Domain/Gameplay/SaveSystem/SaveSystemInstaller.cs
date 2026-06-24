using System;
using Game.Scripts.Domain.App;
using Game.Scripts.Domain.Serializers;
using Zenject;

namespace Game.Scripts.Domain
{
    [Serializable]
    public class SaveSystemInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SaveManager>().AsSingle();

            Container.BindInterfacesAndSelfTo<VersionProvider>().AsSingle();

            Container.Bind<ISaveSerializer>().To<EntityWorldSerializer>().AsCached();
        }
    }
}