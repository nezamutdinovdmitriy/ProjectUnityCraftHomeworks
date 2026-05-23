using System;
using System.Collections.Generic;
using Modules;
using SnakeGame;
using UnityEngine;

namespace GameSystems.Coin
{
    public class CoinManager : IDisposable
    {
        private readonly CoinPool _coinPool;
        private readonly IWorldBounds _worldBounds;
        private readonly List<Modules.Coin> _activeCoins = new();

        public CoinManager(CoinPool pool, IWorldBounds worldBounds)
        {
            _coinPool = pool;
            _worldBounds = worldBounds;
        }

        public IReadOnlyList<Modules.Coin> ActiveCoins => _activeCoins;

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

        public void DespawnCoin(Modules.Coin coin)
        {
            if(_activeCoins.Remove(coin))
                _coinPool.Despawn(coin);
        }
        
        public void ClearActiveCoins()
        {
            for(int i = _activeCoins.Count - 1; i >= 0; i--)
                _coinPool.Despawn(_activeCoins[i]);
            
            _activeCoins.Clear();
        }
    }
}