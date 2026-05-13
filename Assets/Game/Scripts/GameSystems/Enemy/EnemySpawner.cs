using UnityEngine;

namespace Game
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        private Timer _timer;
        
        [SerializeField]
        private EnemyManager _enemyManager;

        private void Update()
        {
            if (_timer.IsReady == false)
                return;
            
            _enemyManager.Spawn();
            
            _timer.Reset();
        }
    }
}