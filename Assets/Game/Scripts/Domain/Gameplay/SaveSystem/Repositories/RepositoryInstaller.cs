using System.IO;
using System.Text;
using Game.Scripts.Domain.Encrypt;
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

        [SerializeField]
        private EncryptedRepositoryConfig _encryptedConfig;
        
        public override void InstallBindings()
        {
            Container
                .Bind<RemoteRepository>()
                .AsSingle()
                .WithArguments(_remoteConfig);

            Container.Bind<FileRepository>()
                .AsSingle()
                .WithArguments(
                Path.Combine(
                    Application.persistentDataPath, 
                    _fileName));
            
            Container
                .Bind<IRepository>()
                .To<SyncRepository>()
                .FromMethod(CreateSyncRepository);
            
            Container
                .Bind<IEncryptor>()
                .To<AesEncryptor>()
                .AsSingle()
                .WithArguments(
                    Encoding.UTF8.GetBytes(_encryptedConfig.Key), 
                    Encoding.UTF8.GetBytes(_encryptedConfig.InitializationVector));
            
            Container.Decorate<IRepository>().With<EncryptedRepository>();
        }

        private SyncRepository CreateSyncRepository() 
            => new(
                Container.Resolve<RemoteRepository>(),
                Container.Resolve<FileRepository>());
    }
}