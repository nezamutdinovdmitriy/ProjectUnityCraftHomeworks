using System;
using GameSystems.Coin;
using Modules;
using Zenject;

namespace GameSystems.Level
{
    public class VictoryGameHandler : IInitializable, IDisposable
    {
        public event Action Victory;

        private readonly LevelManager _levelManger;
        private readonly ISnake _snake;
        private readonly CoinManager _coinManager;

        public VictoryGameHandler(
            LevelManager levelManger,
            ISnake snake,
            CoinManager coinManager)
        {
            _levelManger = levelManger;
            _snake = snake;
            _coinManager = coinManager;
        }

        public void Initialize() => _levelManger.AllLevelsCompleted += HandleVictory;

        public void Dispose() => _levelManger.AllLevelsCompleted -= HandleVictory;

        private void HandleVictory()
        {
            _snake.SetActive(false);
            _coinManager.ClearActiveCoins();
            
            Victory?.Invoke();
        }
    }
}