using GameSystems.Coin;
using GameSystems.Level;
using Modules;
using SnakeGame;
using UI;
using UnityEngine;
using Zenject;

namespace GameSystems
{
    public class SceneContextInstaller : MonoInstaller
    {
        [SerializeField]
        private Snake _snake;

        [SerializeField]
        private GameUI _gameUI;

        [SerializeField]
        private WorldBounds _worldBounds;

        [SerializeField]
        private Modules.Coin _coinPrefab;

        [SerializeField]
        private int _maxLevelCountIndex;

        [SerializeField]
        private int _poolInitialSize;

        [SerializeField]
        private Transform _coinPoolContainer;

        public override void InstallBindings()
        {
            Container
                .Bind<ISnake>()
                .FromInstance(_snake)
                .AsSingle();

            Container
                .Bind<IGameUI>()
                .FromInstance(_gameUI)
                .AsSingle();
            
            Container
                .Bind<IWorldBounds>()
                .FromInstance(_worldBounds)
                .AsSingle();

            Container
                .Bind<IScore>()
                .To<Score>()
                .AsSingle();

            Container
                .Bind<IDifficulty>()
                .To<Difficulty>()
                .AsSingle()
                .WithArguments(_maxLevelCountIndex);

            Container
                .BindMemoryPool<Modules.Coin, CoinPool>()
                .WithInitialSize(_poolInitialSize)
                .FromComponentInNewPrefab(_coinPrefab)
                .UnderTransform(_coinPoolContainer);
            
            Container
                .Bind<ITickable>()
                .To<MovementController>()
                .AsCached();

            Container
                .Bind<IInputProvider>()
                .To<DesktopInputProvider>()
                .AsSingle();

            Container
                .Bind<CoinManager>()
                .AsSingle();
            
            Container
                .BindInterfacesAndSelfTo<DefeatGameHandler>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<LevelManager>()
                .AsSingle();
            
            Container
                .BindInterfacesAndSelfTo<GameOverPresenter>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<ScorePresenter>()
                .AsSingle();
            
            Container
                .BindInterfacesAndSelfTo<DifficultyPresenter>()
                .AsSingle();
        }
    }
}