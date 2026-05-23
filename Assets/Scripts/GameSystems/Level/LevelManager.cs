using System;
using GameSystems.Coin;
using Modules;
using UnityEngine;
using Zenject;

namespace GameSystems.Level
{
    public class LevelManager : IInitializable, IDisposable
    {
        public event Action GameWin;

        private readonly ISnake _snake;
        private readonly CoinManager _coinManager;
        private readonly IDifficulty _difficulty;
        private readonly IScore _score;

        public LevelManager(
            ISnake snake,
            CoinManager coinManager,
            IDifficulty difficulty,
            IScore score)
        {
            _snake = snake;
            _coinManager = coinManager;
            _difficulty = difficulty;
            _score = score;
        }
        
        public void Initialize()
        {
            _snake.OnMoved += OnSnakeMoved;
            _coinManager.AllCoinsCollected += OnAllCoinsCollected;

            StartLevel(_difficulty.Current);
        }

        public void Dispose()
        {
            _snake.OnMoved -= OnSnakeMoved;
            _coinManager.AllCoinsCollected -= OnAllCoinsCollected;
        }
        
        private void OnAllCoinsCollected()
        {
            if (_difficulty.Next(out int nextLevel))
            {
                StartLevel(nextLevel);
            }
            else
            {
                _snake.SetActive(false);
                _coinManager.ClearActiveCoins();
                GameWin?.Invoke();
            }
        }

        private void OnSnakeMoved(Vector2Int headPosition)
        {
            if (_coinManager.CheckCoinCollision(headPosition, out ICoin collectedCoin))
            {
                _snake.Expand(collectedCoin.Bones);
                _score.Add(collectedCoin.Score);
            }
        }
        
        private void StartLevel(int level)
        {
            int mod = level == 0 ? 1 : level;
            
            _coinManager.SpawnCoins(mod);
            _snake.SetSpeed(mod);
        }
    }
}