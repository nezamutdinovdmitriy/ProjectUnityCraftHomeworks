using Game.Scripts.Domain.App.Encrypt.Common;
using Game.Scripts.Domain.App.Hash;
using Game.Scripts.Domain.Encrypt;
using Game.Scripts.Domain.Repositories;
using Zenject;

namespace Game.Scripts.Domain.App.Encrypt
{
    public class EncryptInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<HMACSHA256Provider>()
                .AsSingle()
                .WithArguments(HmacPart1.Key + HmacPart2.Key);

            Container
                .Bind<IEncryptor>()
                .To<AesEncryptor>()
                .AsSingle()
                .WithArguments(AesPart1.Key + AesPart2.Key, AesPart3.IV);
        }
    }
}