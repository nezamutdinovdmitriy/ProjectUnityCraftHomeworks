using UnityEngine;

namespace Game
{
    // +
    public sealed class PlayerBulletInstantiator : MonoBehaviour
    {
        [SerializeField] private BulletManager _bulletManager;

        [SerializeField] private PlayerShip _player;

        private void OnEnable()
        {
            _player.Fired += OnFired;
        }

        private void OnDisable()
        {
            _player.Fired -= OnFired;
        }

        private void OnFired(Ship _)
        {
            _bulletManager.SpawnBullet(
                _player.firePoint.position,
                _player.firePoint.up,
                _player.bulletSpeed,
                _player.bulletDamage,
                _player.Team
            );
        }
    }
}