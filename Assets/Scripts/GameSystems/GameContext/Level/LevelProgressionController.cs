using System;
using GameSystems.Level;
using Zenject;

namespace GameSystems.GameContext.Level
{
    public class LevelProgressionController : IInitializable, IDisposable
    {
        private readonly CoinManager _coinManager;
        private readonly LevelManager _levelManager;

        public LevelProgressionController(CoinManager coinManager, LevelManager levelManager)
        {
            _coinManager = coinManager;
            _levelManager = levelManager;
        }

        public void Initialize() => _coinManager.AllCoinsCollected += OnAllCoinsCollected;

        public void Dispose() => _coinManager.AllCoinsCollected -= OnAllCoinsCollected;

        private void OnAllCoinsCollected() => _levelManager.ProcessLevelCompleted();
    }
}