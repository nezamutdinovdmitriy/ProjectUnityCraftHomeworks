using System;
using GameSystems.Level;
using Zenject;

namespace GameSystems
{
    public class CoinSpawnController : IInitializable, IDisposable
    {
        private readonly CoinManager _coinManager;
        private readonly LevelManager _levelManager;

        public CoinSpawnController(CoinManager coinManager, LevelManager levelManager)
        {
            _coinManager = coinManager;
            _levelManager = levelManager;
        }

        public void Initialize() => _levelManager.LevelStarted += SpawnCoins;

        public void Dispose() => _levelManager.LevelStarted -= SpawnCoins;

        private void SpawnCoins(int modifier) => _coinManager.SpawnCoins(modifier);
    }
}