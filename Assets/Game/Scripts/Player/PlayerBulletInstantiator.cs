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
            _player.FireComponent.Fired += OnFired;
        }

        private void OnDisable()
        {
            _player.FireComponent.Fired -= OnFired;
        }

        private void OnFired(Ship _)
        {
            _bulletManager.SpawnBullet(
                _player.FirePoint.position,
                _player.FirePoint.up,
                _player.BulletSpeed,
                _player.BulletDamage,
                _player.Team
            );
        }
    }
}