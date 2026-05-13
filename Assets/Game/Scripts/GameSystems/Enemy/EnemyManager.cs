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

        public void Spawn() => _pool.Rent();

        public void Despawn(Enemy enemy) => StartCoroutine(DespawnInNextFrame(enemy));

        private IEnumerator DespawnInNextFrame(Enemy enemy)
        {
            yield return null;
            
            EnemyDespawned?.Invoke();
            
            _pool.Push(enemy);
        }
    }
}