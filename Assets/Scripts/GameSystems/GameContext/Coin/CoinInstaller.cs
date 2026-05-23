using System;
using GameSystems.Coin;
using UnityEngine;
using Zenject;

namespace GameSystems
{
    [Serializable]
    public class CoinInstaller : Installer
    {
        [SerializeField]
        private Modules.Coin _coinPrefab;
        
        [SerializeField]
        private int _poolInitialSize;
        
        [SerializeField]
        private Transform _coinPoolContainer;

        public override void InstallBindings()
        {
            Container
                .BindMemoryPool<Modules.Coin, CoinPool>()
                .WithInitialSize(_poolInitialSize)
                .FromComponentInNewPrefab(_coinPrefab)
                .UnderTransform(_coinPoolContainer);

            Container.Bind<CoinManager>().AsSingle();
        }
    }
}