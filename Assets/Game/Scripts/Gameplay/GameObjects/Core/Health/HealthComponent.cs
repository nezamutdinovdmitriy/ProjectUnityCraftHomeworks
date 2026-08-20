using System;
using UnityEngine;

namespace SampleGame
{
    public sealed class HealthComponent : MonoBehaviour
    {
        public event Action<int> OnHealthChanged;
        public event Action OnDeath;
        
        public bool IsAlive => _current > 0;
        public bool IsDead => _current <= 0;
        
        [SerializeField]
        private int _current;

        [SerializeField]
        private int _max;
        
        public bool Decrement(int range)
        {
            if (range < 0)
                throw new Exception($"Range can't be less than zero! Actual range {range}");

            if (_current == 0)
                return false;

            if (range == 0)
                return true;

            _current = Math.Max(0, _current - range);
            this.OnHealthChanged?.Invoke(_current);

            if (_current == 0)
                this.OnDeath?.Invoke();

            return true;
        }
    }
}