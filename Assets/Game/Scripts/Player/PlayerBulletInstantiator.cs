using UnityEngine;

namespace Game
{
    // +
    public sealed class PlayerBulletInstantiator : MonoBehaviour
    {
        [SerializeField]
        private BulletManager _bulletWorld;

        [SerializeField]
        private PlayerShip _player;

        private void OnEnable()
        {
            _player.Fired += OnFired;
        }

        private void OnDisable()
        {
            _player.Fired -= OnFired;
        }

        private void OnFired(ShipController _)
        {
            _bulletWorld.Spawn(
                _player.firePoint.position,
                _player.firePoint.up,
                _player.bulletSpeed,
                _player.bulletDamage,
                _player.Team
            );
        }
    }
}