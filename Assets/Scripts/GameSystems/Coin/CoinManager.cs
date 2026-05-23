using System;
using System.Collections.Generic;
using Modules;
using SnakeGame;
using UnityEngine;

namespace GameSystems.Coin
{
    public class CoinManager : IDisposable
    {
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

        public bool CheckCoinCollision(Vector2Int headPosition, out ICoin collectedCoin)
        {
            for (int i = 0; i < _activeCoins.Count; i++)
            {
                if (_activeCoins[i].Position == headPosition)
                {
                    collectedCoin = _activeCoins[i];
                    
                    _coinPool.Despawn(_activeCoins[i]);
                    _activeCoins.RemoveAt(i);
                    
                    if(_activeCoins.Count == 0)
                        AllCoinsCollected?.Invoke();

                    return true;
                }
            }

            collectedCoin = null;
            return false;
        }
        
        public void ClearActiveCoins()
        {
            for(int i = _activeCoins.Count - 1; i >= 0; i--)
                _coinPool.Despawn(_activeCoins[i]);
            
            _activeCoins.Clear();
        }
    }
}