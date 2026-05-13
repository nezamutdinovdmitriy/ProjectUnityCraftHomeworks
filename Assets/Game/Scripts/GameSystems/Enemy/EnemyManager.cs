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
        
        /*[Space] [SerializeField]
        private ScoreCounter _scoreCounter;*/

        public void Spawn() => _pool.Rent();

        public void Despawn(Enemy enemy)
        {
            /*_scoreCounter.AddScore();*/
            
            StartCoroutine(DespawnInNextFrame(enemy));
        }

        private IEnumerator DespawnInNextFrame(Enemy enemy)
        {
            yield return null;
            
            EnemyDespawned?.Invoke();
            
            _pool.Push(enemy);
        }
    }
}