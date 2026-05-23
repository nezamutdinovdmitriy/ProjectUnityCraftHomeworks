using System;
using GameSystems;
using GameSystems.Level;
using SnakeGame;
using Zenject;

namespace UI
{
    public class GameOverPresenter : IInitializable, IDisposable
    {
        private readonly DefeatGameHandler _defeatHandler;
        private readonly VictoryGameHandler _victoryHandler;
        private readonly IGameUI _gameOverScreen;
        
        public GameOverPresenter(
            DefeatGameHandler defeatHandler, 
            VictoryGameHandler victoryHandler,
            IGameUI gameOverScreen)
        {
            _defeatHandler = defeatHandler;
            _victoryHandler = victoryHandler;
            _gameOverScreen = gameOverScreen;
        }

        public void Initialize()
        {
            _defeatHandler.Defeated += OnDefeated;
            _victoryHandler.Victory += OnVictory;
        }

        public void Dispose()
        {
            _defeatHandler.Defeated -= OnDefeated;
            _victoryHandler.Victory -= OnVictory;
        }

        private void OnVictory() => _gameOverScreen.GameOver(true);
        private void OnDefeated() => _gameOverScreen.GameOver(false);
    }
}