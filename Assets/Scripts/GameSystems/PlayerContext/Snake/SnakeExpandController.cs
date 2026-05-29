using System;
using Modules;
using UnityEngine;
using Zenject;

namespace GameSystems
{
    public class SnakeExpandController : IInitializable, IDisposable
    {
        private readonly CoinManager _coinManager;
        private readonly ISnake _snake;

        public SnakeExpandController(CoinManager coinManager, ISnake snake)
        {
            _coinManager = coinManager;
            _snake = snake;
        }

        public void Initialize() => _coinManager.CoinConsumed += OnCoinConsumed;

        public void Dispose() => _coinManager.CoinConsumed -= OnCoinConsumed;
        
        private void OnCoinConsumed(Modules.Coin coin) => _snake.Expand(coin.Bones);
    }
}