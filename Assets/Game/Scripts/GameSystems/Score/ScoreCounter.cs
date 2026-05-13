using System;
using UnityEngine;

namespace Game
{
    public class ScoreCounter : MonoBehaviour
    {
        public event Action<int> ScoreChanged;

        [SerializeField]
        private EnemyManager _enemyManager;

        private void OnEnable() => _enemyManager.EnemyDespawned += AddScore;

        private void OnDisable() => _enemyManager.EnemyDespawned -= AddScore;

        public int CurrentScore { get; private set; }

        public void AddScore()
        {
            CurrentScore++;
            ScoreChanged?.Invoke(CurrentScore);
        }
    }
}