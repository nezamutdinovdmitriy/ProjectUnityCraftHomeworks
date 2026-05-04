using System;
using System.Collections.Generic;
using Modules.Utils;
using UnityEngine;

namespace Game
{
    // +
    public sealed class BulletManager : MonoBehaviour
    {
        [SerializeField]
        private BulletPool _bulletPool;

        [SerializeField]
        private BulletViewConfig _configView;

        [SerializeField]
        private TransformBounds _levelBounds;

        private readonly List<Bullet> _bullets = new();

        private void FixedUpdate()
        {
            for (int i = _bullets.Count - 1; i >= 0; i--)
            {
                Bullet bullet = _bullets[i];
                Vector3 moveStep = bullet.direction * bullet.speed * Time.fixedDeltaTime;
                bullet.transform.position += moveStep;

                if (_levelBounds.InBounds(bullet.transform.position) == false)
                {
                    _bullets.RemoveAt(i);

                    bullet.TriggerEntered -= this.TriggerEntered;
                    _bulletPool.Push(bullet);
                }
            }
        }

        public void Spawn(Vector2 position, Vector2 direction, float speed, int damage, TeamType team)
        {
            Bullet bullet = _bulletPool.Rent();
            
            bullet.direction = direction;
            bullet.speed = speed;
            bullet.damage = damage;
            bullet.team = team;

            bullet.transform.position = position;
            bullet.transform.rotation = Quaternion.LookRotation(direction, Vector3.forward);
            bullet.gameObject.layer = team switch
            {
                TeamType.None => LayerMask.NameToLayer("Default"),
                TeamType.Player => LayerMask.NameToLayer("PlayerBullet"),
                TeamType.Enemy => LayerMask.NameToLayer("EnemyBullet"),
                _ => throw new ArgumentOutOfRangeException(nameof(team), team, null)
            };

            if (team == TeamType.Player)
            {
                bullet.blueVFX.SetActive(true);
                bullet.redVFX.SetActive(false);
            }
            else
            {
                bullet.blueVFX.SetActive(false);
                bullet.redVFX.SetActive(true);
            }

            bullet.TriggerEntered += this.TriggerEntered;
            _bullets.Add(bullet);
        }

        private void TriggerEntered(Bullet bullet, Collider2D other)
        {
            if (other.TryGetComponent(out ShipController ship) == false) 
                return;

            if (bullet.team == TeamType.Player && ship is Enemy ||
                bullet.team == TeamType.Enemy && ship is PlayerShip)
            {
                // Deal damage to target:
                if (bullet.damage > 0)
                {
                    ship.currentHealth = Mathf.Clamp(ship.currentHealth - bullet.damage, 0, ship.config.Health);
                    ship.NotifyAboutHealthChanged(ship.currentHealth);
 
                    if (ship.currentHealth <= 0)
                    {
                        ship.NotifyAboutDead();
                        ship.gameObject.SetActive(false);
                    }
                }

                bullet.TriggerEntered -= this.TriggerEntered;

                _bullets.Remove(bullet);
                
                _bulletPool.Push(bullet);

                // Explosion Vfx
                GameObject prefab = _configView.ExplosionVFX;
                Instantiate(prefab, bullet.transform.position, prefab.transform.rotation);
            }
        }
    }
}