using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EnemyPool : MonoBehaviour
    {
        private const int PrewarmCount  = 10;
        
        [SerializeField]
        private EnemyFactory _enemyFactory;

        private readonly Stack<Enemy> _stack = new();

        private void Awake()
        {
            for (int i = 0; i < PrewarmCount ; i++)
            {
                Enemy enemy = _enemyFactory.Create();
                enemy.gameObject.SetActive(false);
                _stack.Push(enemy);
            }
        }

        public Enemy Rent()
        {
            if (_stack.TryPop(out Enemy enemy))
            {
                enemy.gameObject.SetActive(true);
                return enemy;
            }

            return _enemyFactory.Create();
        }
        
        public void Push(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
            _stack.Push(enemy);
        }
    }
}