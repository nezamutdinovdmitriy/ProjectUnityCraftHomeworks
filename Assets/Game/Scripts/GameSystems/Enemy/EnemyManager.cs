using System;
using System.Collections;
using Game.Spawn;
using UnityEngine;

namespace Game
{
    // +
    public sealed class EnemyManager : MonoBehaviour, IEnemyDespawner
    {
        public event Action EnemyDespawned;
        
        [Header("Spawn")] [SerializeField]
        private Pool<Enemy> _pool;

        [SerializeField]
        private EnemyPositions _enemyPositions;

        public void Spawn()
        {
            Enemy enemy = _pool.Rent();
            
            enemy.transform.position = _enemyPositions.NextSpawnPosition();
            
            enemy.Initialize(_enemyPositions.NextDestination());
        }

        public void Despawn(Enemy enemy) => StartCoroutine(DespawnInNextFrame(enemy));

        private IEnumerator DespawnInNextFrame(Enemy enemy)
        {
            yield return null;
            
            EnemyDespawned?.Invoke();
            
            _pool.Push(enemy);
        }
    }
}