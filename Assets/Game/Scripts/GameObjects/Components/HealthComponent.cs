using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class HealthComponent
    {
        public event Action<int> Changed;
        public event Action Dead;

        public int Current { get; private set; }
        public int Max { get; private set; }

        public bool IsDead => Current <= 0;

        public void SetMaxHealth(int value) => Max = value;
        public void SetCurrentHealth(int value) => Current = value;
        
        public void TakeDamage(int damage)
        {
            if (Current <= 0 || damage <= 0)
                return;

            Current = Mathf.Max(0, Current - damage);
            Changed?.Invoke(Current);

            if (Current == 0)
                Dead?.Invoke();
        }
    }
}