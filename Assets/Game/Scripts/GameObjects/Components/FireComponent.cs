using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class FireComponent
    {
        public event Action<Ship> Fired;

        [SerializeField]
        private BulletManager _bulletManager;
        
        private float _cooldown;
        private float _lastFireTime;

        public void Initialize(float cooldown)
        {
            _cooldown = cooldown;
        }
        
        public void Execute(Ship owner, Vector2 direction, bool canFire)
        {
            if (canFire == false)
                return;

            float time = Time.time;

            if (time - _lastFireTime < _cooldown)
                return;
            
            Fire(owner, direction);
            
            _lastFireTime = time;
            Fired?.Invoke(owner);
        }

        private void Fire(Ship ship, Vector2 direction)
        {
            _bulletManager.SpawnBullet(
                ship.FirePoint.position,
                direction,
                TeamType.Enemy
            ); 
        }
    }
}