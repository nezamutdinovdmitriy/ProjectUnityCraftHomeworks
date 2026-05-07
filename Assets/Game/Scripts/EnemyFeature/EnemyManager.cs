using System.Collections;
using Modules.UI;
using UnityEngine;

namespace Game
{
    // +
    public sealed class EnemyManager : MonoBehaviour, IEnemyDespawner
    {
        [Header("Target")] [SerializeField]
        private Ship _player;

        [Header("Bullets")] [SerializeField]
        private BulletManager _bulletManager;

        [Header("Spawn")] [SerializeField]
        private EnemySpawner _enemySpawner;

        [Header("UI")] [SerializeField]
        private ScoreView _scoreView;

        private int _destroyedEnemies;

        private void Awake() => _scoreView.SetValue(_destroyedEnemies);
        private void OnEnable() => _enemySpawner.Spawned += OnEnemySpawned;
        private void OnDisable() => _enemySpawner.Spawned -= OnEnemySpawned;

        private void FixedUpdate()
        {
            _enemySpawner.Tick(_player.HealthComponent.Current > 0);
        }
        
        private void OnEnemySpawned(Enemy enemy)
        {
            enemy.ResetHealth();
            enemy.target = _player;
            enemy.SetDespawner(this);
            enemy.FireComponent.Fired += OnEnemyFired;
        }

        private void OnEnemyFired(Ship enemy)
        {
            Vector2 position = enemy.FirePoint.position;
            Vector2 target = _player.transform.position;
            Vector2 direction = (target - position).normalized;
            _bulletManager.SpawnBullet(
                enemy.FirePoint.position,
                direction,
                enemy.BulletSpeed,
                enemy.BulletDamage,
                TeamType.Enemy
            );
        }
        
        public void Despawn(Enemy enemy)
        {
            _destroyedEnemies++;
            _scoreView.SetValue(_destroyedEnemies);

            enemy.FireComponent.Fired -= OnEnemyFired;

            StartCoroutine(DespawnInNextFrame(enemy));
        }

        private IEnumerator DespawnInNextFrame(Enemy enemy)
        {
            yield return null;
            enemy.gameObject.SetActive(false);
            _enemySpawner.Despawn(enemy);
        }
    }
}