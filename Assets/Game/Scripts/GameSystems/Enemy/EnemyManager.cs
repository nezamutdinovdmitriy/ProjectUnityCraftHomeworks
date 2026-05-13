using System.Collections;
using Game.Spawn;
using UnityEngine;

namespace Game
{
    // +
    public sealed class EnemyManager : MonoBehaviour, IEnemyDespawner
    {
        [Header("Spawn")] [SerializeField]
        private Pool<Enemy> _pool;
        
        [SerializeField]
        private EnemyPositions _enemyPositions;
        
        [Space] [SerializeField]
        private ScoreCounter _scoreCounter;

        public void Spawn()
        {
            Enemy enemy = _pool.Rent();

            enemy.transform.position = _enemyPositions.NextSpawnPosition();
            
            enemy.Initialize(_enemyPositions.NextDestination(), this);
        }
        
        public void Despawn(Enemy enemy)
        {
            _scoreCounter.AddScore();
            
            StartCoroutine(DespawnInNextFrame(enemy));
        }

        private IEnumerator DespawnInNextFrame(Enemy enemy)
        {
            yield return null;
            
            _pool.Push(enemy);
        }
    }
}