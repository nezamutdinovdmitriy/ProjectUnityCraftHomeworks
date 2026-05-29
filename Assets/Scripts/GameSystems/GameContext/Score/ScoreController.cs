using System;
using Modules;
using UnityEngine;
using Zenject;

namespace GameSystems.GameContext
{
    public class ScoreController : IInitializable, IDisposable
    {
        private readonly CoinManager _coinManager;
        private readonly IScore _score;

        public ScoreController(CoinManager coinManager, IScore score)
        {
            _coinManager = coinManager;
            _score = score;
        }

        public void Initialize() => _coinManager.CoinConsumed += OnCoinConsumed;

        public void Dispose() => _coinManager.CoinConsumed -= OnCoinConsumed;

        private void OnCoinConsumed(Modules.Coin coin) => _score.Add(coin.Score);
    }
}