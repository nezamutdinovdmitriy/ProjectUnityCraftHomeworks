using System;
using System.Collections.Generic;
using SnakeGame;
using UnityEngine;

namespace GameSystems
{
    public class CoinManager : IDisposable
    {
        public event Action<Modules.Coin> CoinConsumed;
        public event Action AllCoinsCollected;

        private readonly CoinPool _coinPool;
        private readonly IWorldBounds _worldBounds;
        private readonly List<Modules.Coin> _activeCoins = new();

        public CoinManager(CoinPool pool, IWorldBounds worldBounds)
        {
            _coinPool = pool;
            _worldBounds = worldBounds;
        }

        public void Dispose() => ClearActiveCoins();

        public void SpawnCoins(int count)
        {
            ClearActiveCoins();

            for (int i = 0; i < count; i++)
            {
                Vector2Int randomPosition = _worldBounds.GetRandomPosition();

                Modules.Coin coin = _coinPool.Spawn(randomPosition);
                _activeCoins.Add(coin);
            }
        }

        public bool TryConsumeCoin(Vector2Int consumerPosition)
        {
            for (int i = _activeCoins.Count - 1; i >= 0; i--)
            {
                Modules.Coin coin = _activeCoins[i];

                if (coin.Position == consumerPosition)
                {
                    ConsumeCoin(coin);

                    if (_activeCoins.Count == 0)
                        AllCoinsCollected?.Invoke();

                    return true;
                }
            }

            return false;
        }

        public void ClearActiveCoins()
        {
            for (int i = _activeCoins.Count - 1; i >= 0; i--)
            {
                if (_activeCoins[i] != null)
                    _coinPool.Despawn(_activeCoins[i]);
            }

            _activeCoins.Clear();
        }

        private void ConsumeCoin(Modules.Coin coin)
        {
            _activeCoins.Remove(coin);
            _coinPool.Despawn(coin);
            CoinConsumed?.Invoke(coin);
        }
    }
}