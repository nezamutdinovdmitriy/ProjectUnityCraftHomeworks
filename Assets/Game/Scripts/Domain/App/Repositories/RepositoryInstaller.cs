using System.IO;
using Game.Scripts.Domain.App.Hash;
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
        private string _fileName;
        
        [SerializeField]
        private RemoteRepositoryConfig _remoteConfig;
        
        public override void InstallBindings()
        {
            Container
                .Bind<RemoteRepository>()
                .AsSingle()
                .WithArguments(_remoteConfig);

            Container
                .Bind<FileRepository>()
                .AsSingle()
                .WithArguments(
                Path.Combine(
                    Application.persistentDataPath, 
                    _fileName));
            
            Container
                .Bind<IRepository>()
                .To<SyncRepository>()
                .FromMethod(CreateSyncRepository)
                .AsSingle();

            Container.Decorate<IRepository>().With<CheckSumRepository>();
        }

        private SyncRepository CreateSyncRepository()
        {
            FileRepository fileRepository = Container.Resolve<FileRepository>();
            RemoteRepository remoteRepository = Container.Resolve<RemoteRepository>();
            
            // IHashProvider hashProvider = Container.Resolve<IHashProvider>();
            // IRepository securedFile = new CheckSumRepository(fileRepository, hashProvider);
            // IRepository securedRemote = new CheckSumRepository(remoteRepository, hashProvider);
        
            return new SyncRepository(fileRepository, remoteRepository);
        }
    }
}