using System;
using Modules;
using UnityEngine;
using Zenject;

namespace GameSystems.GameContext.Coin
{
    public class CoinPickupController : IInitializable, IDisposable
    {
        private readonly CoinManager _coinManager;
        private readonly ISnake _snake;

        public CoinPickupController(CoinManager coinManager, ISnake snake)
        {
            _coinManager = coinManager;
            _snake = snake;
        }

        public void Initialize() => _snake.OnMoved += OnMoved;

        public void Dispose() => _snake.OnMoved -= OnMoved;

        private void OnMoved(Vector2Int position) => _coinManager.TryConsumeCoin(position);
    }
}