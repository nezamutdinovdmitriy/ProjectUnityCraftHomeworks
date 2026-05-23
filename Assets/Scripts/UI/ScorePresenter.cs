using System;
using Modules;
using SnakeGame;
using Zenject;

namespace UI
{
    public class ScorePresenter : IInitializable, IDisposable
    {
        private readonly IScore _score;
        private readonly IGameUI _gameUI;

        public ScorePresenter(IScore score, IGameUI gameUI)
        {
            _score = score;
            _gameUI = gameUI;
        }

        public void Initialize()
        {
            _score.OnStateChanged += UpdateScore;

            UpdateScore(_score.Current);
        }

        public void Dispose() => _score.OnStateChanged -= UpdateScore;

        private void UpdateScore(int currentScore) => _gameUI.SetScore(currentScore.ToString());
    }
}