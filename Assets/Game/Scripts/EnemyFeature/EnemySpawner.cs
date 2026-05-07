using System;
using Modules.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class EnemySpawner : MonoBehaviour
    {
        public event Action<Enemy, Vector2> Spawned;
        
        [Header("Spawn")] 
        [SerializeField]
        private float _minSpawnCooldown = 2;
        [SerializeField]
        private float _maxSpawnCooldown = 3;

        [Header("Points")] 
        [SerializeField]
        private Transform[] _spawnPositions;
        [SerializeField]
        private Transform[] _attackPositions;
        
        [Header("Pool")] 
        [SerializeField]
        private EnemyPool _pool;
        
        private float _spawnCooldown;
        private float _spawnTime;
        
        private int _spawnIndex;
        private int _attackIndex;

        private void Awake()
        {
            _spawnPositions.Shuffle();
            _attackPositions.Shuffle();
        }

        private void Start() => ResetSpawnCooldown();
        
        public void Tick(bool canSpawn)
        {
            if (canSpawn == false)
                return;

            float time = Time.fixedTime;
            
            if (time - _spawnTime < _spawnCooldown)
                return;
            
            Spawn();
            ResetSpawnCooldown();
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
        
        private void ResetSpawnCooldown()
        {
            _spawnCooldown = Random.Range(_minSpawnCooldown, _maxSpawnCooldown);
            _spawnTime = Time.fixedTime;
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