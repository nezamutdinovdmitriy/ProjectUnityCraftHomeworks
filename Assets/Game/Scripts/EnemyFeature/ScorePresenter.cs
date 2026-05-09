using Modules.UI;
using UnityEngine;

namespace Game
{
    public class ScorePresenter : MonoBehaviour
    {
        [SerializeField]
        private ScoreView _view;

        [SerializeField]
        private ScoreController _scoreController;

        private void Awake()
        {
            _scoreController.ScoreChanged += OnScoreChanged;
            _view.SetValue(_scoreController.CurrentScore);
        }

        private void OnDisable() => _scoreController.ScoreChanged -= OnScoreChanged;

        private void OnScoreChanged(int value) => _view.SetValue(value);
    }
}