using Modules.UI;
using UnityEngine;

namespace Game
{
    public class ScorePresenter : MonoBehaviour
    {
        [SerializeField]
        private ScoreView _view;

        [SerializeField]
        private ScoreCounter scoreCounter;

        private void Awake()
        {
            scoreCounter.ScoreChanged += OnScoreChanged;
            _view.SetValue(scoreCounter.CurrentScore);
        }

        private void OnDisable() => scoreCounter.ScoreChanged -= OnScoreChanged;

        private void OnScoreChanged(int value) => _view.SetValue(value);
    }
}