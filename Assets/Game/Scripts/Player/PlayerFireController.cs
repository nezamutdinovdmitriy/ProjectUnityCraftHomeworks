using UnityEngine;

namespace Game
{
    // +
    public sealed class PlayerFireController : MonoBehaviour
    {
        [SerializeField]
        private BulletManager _bulletManager;

        [SerializeField]
        private Ship _ship;

        private void OnEnable() => _ship.FireComponent.Fired += OnPlayerFired;
        private void OnDisable() => _ship.FireComponent.Fired -= OnPlayerFired;

        private void OnPlayerFired(Ship _)
        {
            _bulletManager.SpawnBullet(
                _ship.FirePoint.position,
                _ship.FirePoint.up,
                _ship.BulletSpeed,
                _ship.BulletDamage,
                _ship.Team
            );
        }
    }
}