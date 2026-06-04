using System;
using GameSystems.GameContext.Level;
using GameSystems.GameContext.Level.LevelResultHandlers;
using GameSystems.Level;
using Modules;
using SnakeGame;
using UnityEngine;
using Zenject;

namespace GameSystems
{
    [Serializable]
    public class LevelInstaller : Installer
    {
        [SerializeField]
        private int _maxLevelCountIndex;

        [SerializeField]
        private WorldBounds _worldBounds;

        public override void InstallBindings()
        {
            Container.Bind<IScore>().To<Score>().AsSingle();

            Container.Bind<IDifficulty>().To<Difficulty>().AsSingle().WithArguments(_maxLevelCountIndex);

            Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle();

            Container.BindInterfacesAndSelfTo<DefeatGameController>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<VictoryGameController>().AsSingle();
            
            Container.Bind<IWorldBounds>().FromInstance(_worldBounds).AsSingle();

            Container.BindInterfacesTo<LevelProgressionController>().AsSingle();

            Container.BindInterfacesTo<DefeatGameHandler>().AsSingle();
            
            Container.BindInterfacesTo<VictoryGameHandler>().AsSingle();
        }
    }
}