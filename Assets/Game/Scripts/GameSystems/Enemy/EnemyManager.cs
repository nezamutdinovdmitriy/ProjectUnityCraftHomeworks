using System.Collections;
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

        [Space] [SerializeField]
        private ScoreCounter _scoreCounter;

        private void OnEnable() => _enemySpawner.Spawned += OnEnemySpawned;
        private void OnDisable() => _enemySpawner.Spawned -= OnEnemySpawned;

        private void FixedUpdate()
        {
            _enemySpawner.Tick(_player.HealthComponent.Current > 0);
        }

        private void OnEnemySpawned(Enemy enemy, Vector2 destination)
        {
            enemy.Initialize(_player, destination, this);

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
                TeamType.Enemy
            );
        }

        public void Despawn(Enemy enemy)
        {
            _scoreCounter.AddScore();

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