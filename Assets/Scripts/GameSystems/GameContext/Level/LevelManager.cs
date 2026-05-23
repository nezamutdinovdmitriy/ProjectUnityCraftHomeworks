using System;
using System.Collections.Generic;
using GameSystems.Coin;
using Modules;
using UnityEngine;
using Zenject;

namespace GameSystems.Level
{
    public class LevelManager : IInitializable, IDisposable
    {
        public event Action AllLevelsCompleted;

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
            StartLevel(_difficulty.Current);
        }

        public void Dispose() => _snake.OnMoved -= OnSnakeMoved;

        private void OnSnakeMoved(Vector2Int headPosition)
        {
            IReadOnlyList<Modules.Coin> activeCoins = _coinManager.ActiveCoins;

            for (int i = activeCoins.Count - 1; i >= 0; i--)
            {
                if (activeCoins[i].Position == headPosition)
                {
                    _snake.Expand(activeCoins[i].Bones);
                    _score.Add(activeCoins[i].Score);
                    _coinManager.DespawnCoin(activeCoins[i]);
                }
            }
            
            if (_coinManager.ActiveCoins.Count == 0)
                ProcessLevelCompleted();
        }

        private void ProcessLevelCompleted()
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