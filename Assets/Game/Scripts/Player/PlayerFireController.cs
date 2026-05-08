using UnityEngine;

namespace Game
{
    // +
    public sealed class PlayerFireController : MonoBehaviour
    {
        [SerializeField]
        private BulletManager _bulletManager;

        [SerializeField]
        private PlayerShip _player;

        private void OnEnable() => _player.FireComponent.Fired += OnPlayerFired;
        private void OnDisable() => _player.FireComponent.Fired -= OnPlayerFired;

        private void OnPlayerFired(Ship _)
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