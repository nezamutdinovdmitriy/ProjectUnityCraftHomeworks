using System;
using GameSystems.GameContext;
using SnakeGame;
using Zenject;

namespace UI
{
    public class GameOverPresenter : IInitializable, IDisposable
    {
        private readonly GameCycle _gameCycle;
        private readonly IGameUI _gameOverScreen;
        
        public GameOverPresenter(
            GameCycle gameCycle,
            IGameUI gameOverScreen)
        {
            _gameCycle = gameCycle;
            _gameOverScreen = gameOverScreen;
        }

        public void Initialize()
        {
            _gameCycle.Defeated += OnDefeated;
            _gameCycle.Victory += OnVictory;
        }

        public void Dispose()
        {
            _gameCycle.Defeated -= OnDefeated;
            _gameCycle.Victory -= OnVictory;
        }

        private void OnVictory() => _gameOverScreen.GameOver(true);
        private void OnDefeated() => _gameOverScreen.GameOver(false);
    }
}