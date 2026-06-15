using UnityEngine;
using Zenject;

namespace Game.Scripts.Domain.Repositories
{
    [CreateAssetMenu(
        fileName = "RemoteRepositoryInstaller",
        menuName = "Installers/New RemoteRepositoryInstaller")]
    public class RepositoryInstaller : ScriptableObjectInstaller
    {
        [SerializeField]
        private RemoteRepositoryConfig _config;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_config).AsSingle();
            Container.Bind<IRepository>().To<RemoteRepository>().AsSingle();
        }
    }
}