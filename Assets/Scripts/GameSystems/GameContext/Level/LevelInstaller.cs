using System;
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

            Container.BindInterfacesAndSelfTo<DefeatGameHandler>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<VictoryGameHandler>().AsSingle();
            
            Container.Bind<IWorldBounds>().FromInstance(_worldBounds).AsSingle();
        }
    }
}