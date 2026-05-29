using Extensions;
using UnityEngine;
using Zenject;

namespace GameSystems.GameContext
{
    public class GameContextInstaller : MonoInstaller
    {
        [SerializeField]
        private CoinInstaller _coinInstaller;

        [SerializeField]
        private LevelInstaller _levelInstaller;
        
        public override void InstallBindings()
        {
            Container
                .Install(_coinInstaller)
                .Install(_levelInstaller);

            Container.BindInterfacesTo<ScoreController>().AsSingle();
            Container.Bind<GameCycle>().AsSingle();
        }
    }
}