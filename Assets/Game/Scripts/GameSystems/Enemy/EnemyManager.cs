using System.Collections;
using UnityEngine;

namespace Game
{
    // +
    public sealed class EnemyManager : MonoBehaviour, IEnemyDespawner
    {
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
            // _enemySpawner.Tick(_player.HealthComponent.Current > 0);
            _enemySpawner.Tick(true);
        }

        private void OnEnemySpawned(Enemy enemy, Vector2 destination)
        {
            enemy.Initialize(destination, _bulletManager, this);
        }

        public void Despawn(Enemy enemy)
        {
            _scoreCounter.AddScore();
            
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