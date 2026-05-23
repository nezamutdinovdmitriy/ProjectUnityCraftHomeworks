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
        private readonly LevelManager _levelManger;
        private readonly IGameUI _gameOverScreen;
        
        public GameOverPresenter(
            DefeatGameHandler defeatHandler, 
            IGameUI gameOverScreen, 
            LevelManager levelManger)
        {
            _defeatHandler = defeatHandler;
            _gameOverScreen = gameOverScreen;
            _levelManger = levelManger;
        }

        public void Initialize()
        {
            _defeatHandler.Defeated += OnDefeated;
            _levelManger.GameWin += OnWin;
        }

        public void Dispose()
        {
            _defeatHandler.Defeated -= OnDefeated;
            _levelManger.GameWin -= OnWin;
        }

        private void OnWin() => _gameOverScreen.GameOver(true);
        
        private void OnDefeated() => _gameOverScreen.GameOver(false);
    }
}