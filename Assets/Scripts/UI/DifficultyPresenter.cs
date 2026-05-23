using System;
using Modules;
using SnakeGame;
using Zenject;

namespace UI
{
    public class DifficultyPresenter :  IInitializable, IDisposable
    {
        private readonly IDifficulty _difficulty;
        private readonly IGameUI _gameUI;

        public DifficultyPresenter(IDifficulty difficulty, IGameUI gameUI)
        {
            _difficulty = difficulty;
            _gameUI = gameUI;
        }
        
        public void Initialize()
        {
            _difficulty.OnStateChanged += UpdateDifficulty;
            
            UpdateDifficulty();
        }

        public void Dispose() => _difficulty.OnStateChanged -= UpdateDifficulty;
        
        private void UpdateDifficulty() => _gameUI.SetDifficulty(_difficulty.Current, _difficulty.Max + 1);
    }
}