using System;
using SnakeGame;
using UnityEngine;
using Zenject;

namespace UI
{
    [Serializable]
    public class UIContextInstaller : MonoInstaller
    {
        [SerializeField]
        private GameUI _gameUI;

        public override void InstallBindings()
        {
            Container.Bind<IGameUI>().FromInstance(_gameUI).AsSingle();

            Container.BindInterfacesAndSelfTo<GameOverPresenter>().AsSingle();

            Container.BindInterfacesAndSelfTo<ScorePresenter>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<DifficultyPresenter>().AsSingle();
        }
    }
}