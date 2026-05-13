using Game.Spawn;
using UnityEngine;

namespace Game
{
    public class EnemyPool : Pool<Enemy>
    {
        [SerializeField]
        private EnemyPositions _enemyPositions;

        public override Enemy Rent()
        {
            Enemy enemy = base.Rent();
            
            enemy.transform.position = _enemyPositions.NextSpawnPosition();
            
            enemy.Initialize(_enemyPositions.NextDestination());

            return enemy;
        }
    }
}