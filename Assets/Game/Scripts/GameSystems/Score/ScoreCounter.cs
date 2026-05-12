using System;
using UnityEngine;

namespace Game
{
    public class ScoreCounter : MonoBehaviour
    {
        public event Action<int> ScoreChanged;
        
        public int CurrentScore { get; private set; }

        public void AddScore()
        {
            CurrentScore++;
            ScoreChanged?.Invoke(CurrentScore);
        }
    }
}