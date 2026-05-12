using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    [Serializable]
    public class Timer
    {
        [Header("Intervals")] 
        [SerializeField]
        private float _minInterval = 2f;
        [SerializeField]
        private float _maxInterval = 3f;
        
        private float _currentInterval;
        private float _lastTickTime;

        public bool IsReady => Time.fixedTime - _lastTickTime >= _currentInterval; 
        
        public void Reset()
        {
            _currentInterval = Random.Range(_minInterval, _maxInterval);
            _lastTickTime = Time.fixedTime;
        }
    }
}