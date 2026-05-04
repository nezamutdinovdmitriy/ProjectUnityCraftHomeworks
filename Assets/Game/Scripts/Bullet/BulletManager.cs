using System;
using System.Collections.Generic;
using Modules.Utils;
using UnityEngine;

namespace Game
{
    // +
    public sealed class BulletManager : MonoBehaviour
    {
        private readonly List<Bullet> _bullets = new();
        
        [SerializeField]
        private BulletPool _bulletPool;

        [SerializeField]
        private BulletViewConfig _configView;

        [SerializeField]
        private TransformBounds _levelBounds;
        
        public void Spawn(Vector2 position, Vector2 direction, float speed, int damage, TeamType team)
        {
            Bullet bullet = _bulletPool.Rent();
            
            bullet.Initialize(position, direction, damage, speed, team);

            bullet.Hit += OnHit;
            
            _bullets.Add(bullet);
        }
        
        private void FixedUpdate()
        {
            for (int i = _bullets.Count - 1; i >= 0; i--)
            {
                Bullet bullet = _bullets[i];
                bullet.MoveStep(Time.fixedDeltaTime);

                if (_levelBounds.InBounds(bullet.transform.position) == false)
                {
                    _bullets.RemoveAt(i);

                    ReleaseBullet(bullet);
                }
            }
        }

        private void OnHit(Bullet bullet, Collider2D other)
        {
            _bullets.Remove(bullet);

            ReleaseBullet(bullet);

            // Explosion Vfx
            GameObject prefab = _configView.ExplosionVFX;
            Instantiate(prefab, bullet.transform.position, prefab.transform.rotation);
        }

        private void ReleaseBullet(Bullet bullet)
        {
            bullet.Hit -= OnHit;
            _bulletPool.Push(bullet);
        }
    }
}