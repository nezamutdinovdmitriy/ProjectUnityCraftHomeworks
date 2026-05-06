using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class FireComponent
    {
        public event Action<Ship> Fired;

        private float _cooldown;
        private float _lastFireTime;
        
        public void Initialize(float cooldown)
        {
            _cooldown = cooldown;
        }

        public void Execute(Ship owner)
        {
            float time = Time.time;
            
            if (time - _lastFireTime < _cooldown)
                return;

            _lastFireTime = time;
            Fired?.Invoke(owner);
        }
    }
}