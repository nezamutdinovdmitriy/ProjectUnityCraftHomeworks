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
            _player.OnFire += OnFire;
        }

        private void OnDisable()
        {
            _player.OnFire -= OnFire;
        }

        private void OnFire(ShipController _)
        {
            _bulletWorld.Spawn(
                _player.firePoint.position,
                _player.firePoint.up,
                _player.bulletSpeed,
                _player.bulletDamage,
                TeamType.Player
            );
        }
    }
}