using System;
using Game.Scripts.Utilities;
using Modules.Utils;
using UnityEngine;

namespace Game
{
    public class EnemySpawner : MonoBehaviour
    {
        public event Action<Enemy, Vector2> Spawned;

        [SerializeField]
        private Timer _timer;

        [Header("Points")] 
        [SerializeField]
        private Transform[] _spawnPositions;
        [SerializeField]
        private Transform[] _attackPositions;
        
        [Header("Pool")] 
        [SerializeField]
        private EnemyPool _pool;
        
        private int _spawnIndex;
        private int _attackIndex;

        private void Awake()
        {
            _spawnPositions.Shuffle();
            _attackPositions.Shuffle();
        }
        
        public void Tick(bool canSpawn)
        {
            if (canSpawn == false || _timer.IsReady == false)
                return;
            
            Spawn();
            _timer.Reset();
        }
        
        private void Spawn()
        {
            Enemy enemy = _pool.Rent();

            Vector3 spawnPosition = NextSpawnPosition();
            Vector2 attackPosition = NextDestination();

            enemy.transform.position = spawnPosition;
            
            Spawned?.Invoke(enemy, attackPosition);
        }

        public void Despawn(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
            _pool.Push(enemy);
        }
        
        private Vector3 NextSpawnPosition()
        {
            if (_spawnIndex >= _spawnPositions.Length)
            {
                _spawnPositions.Shuffle();
                _spawnIndex = 0;
            }

            return _spawnPositions[_spawnIndex++].position;
        }

        private Vector3 NextDestination()
        {
            if (_attackIndex >= _attackPositions.Length)
            {
                _attackPositions.Shuffle();
                _attackIndex = 0;
            }

            return _attackPositions[_attackIndex++].position;
        }
    }
}