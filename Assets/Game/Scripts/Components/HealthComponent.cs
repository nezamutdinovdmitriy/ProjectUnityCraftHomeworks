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

        public void Initialize(int maxHealth)
        {
            Max = maxHealth;
            Current = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (Current <= 0 || damage <= 0)
                return;

            Current = Mathf.Max(0, Current - damage);
            Changed?.Invoke(Current);

            Debug.Log("CURRENT HP: " + Current);
            
            if (Current == 0)
            {
                Debug.Log("+");
                Dead?.Invoke();
            }
        }
    }
}