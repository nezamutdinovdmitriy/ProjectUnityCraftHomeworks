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
            Enemy instance = _stack.TryPop(out Enemy enemy) ? enemy : _enemyFactory.Create();

            instance.gameObject.SetActive(true);
            
            Debug.Log("Enemy Rent");
            
            return instance;
        }
        
        public void Push(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
            _stack.Push(enemy);
            
            Debug.Log("Enemy Push");
        }
    }
}