using System;
using Modules;
using Zenject;

namespace GameSystems.Level
{
    public class LevelManager : IInitializable
    {
        public event Action AllLevelsCompleted;

        private readonly ISnake _snake;
        private readonly CoinManager _coinManager;
        private readonly IDifficulty _difficulty;

        public LevelManager(
            ISnake snake,
            CoinManager coinManager,
            IDifficulty difficulty)
        {
            _snake = snake;
            _coinManager = coinManager;
            _difficulty = difficulty;
        }
        
        public void Initialize() => StartLevel(_difficulty.Current);
        
        public void ProcessLevelCompleted()
        {
            if (_difficulty.Next(out int nextLevel))
                StartLevel(nextLevel);
            else
                AllLevelsCompleted?.Invoke();
        }

        private void StartLevel(int level)
        {
            int modifier = level == 0 ? 1 : level;
            
            _coinManager.SpawnCoins(modifier);
            _snake.SetSpeed(modifier);
        }
    }
}