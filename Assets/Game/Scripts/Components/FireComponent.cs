using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class FireComponent
    {
        public event Action Fired;
        private float _lastFireTime;

        public void Execute(float cooldown, bool isAlive)
        {
            if (isAlive == false)
                return;
            
            float time = Time.time;
            if (time - _lastFireTime < cooldown)
                return;

            _lastFireTime = time;

            Fired?.Invoke();
        }
    }
}