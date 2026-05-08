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
        private TransformBounds _levelBounds;

        private void FixedUpdate()
        {
            for (int i = _bullets.Count - 1; i >= 0; i--)
            {
                Bullet bullet = _bullets[i];

                bullet.MoveStep(Time.fixedDeltaTime);

                if (_levelBounds.InBounds(bullet.transform.position) == false)
                    ReleaseBullet(bullet);
            }
        }

        public void SpawnBullet(
            Vector2 position,
            Vector2 direction,
            float speed,
            int damage,
            TeamType team)
        {
            Bullet bullet = _bulletPool.Rent();

            bullet.Initialize(position, direction, damage, speed, team);

            bullet.Hit += OnBulletHit;
            _bullets.Add(bullet);
        }

        private void OnBulletHit(Bullet bullet, Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable target)
                && target.Team != bullet.Team)
                target.TakeDamage(bullet.Damage);
            
            ReleaseBullet(bullet);
        }

        private void ReleaseBullet(Bullet bullet)
        {
            bullet.Hit -= OnBulletHit;
            _bullets.Remove(bullet);
            _bulletPool.Push(bullet);
        }
    }
}