using UnityEngine;
 
 namespace Game
 {
     public class EnemySpawner : MonoBehaviour
     {
         [SerializeField]
         private Timer _timer;
         
         [SerializeField]
         private EnemyManager _enemyManager;
 
         [SerializeField]
         private Ship _playerShip;
 
         private void Update()
         {
             if (_playerShip.HealthComponent.IsDead == true || _timer.IsReady == false)
                 return;
             
             _enemyManager.Spawn();
             
             _timer.Reset();
         }
     }
 }