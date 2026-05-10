using UnityEngine;

namespace Game
{
    public class EnemyCombatAI : MonoBehaviour
    {
        [SerializeField]
        private EnemyNavigationAI _navigation;
        
        private Enemy _enemy;
        
        [Header("AI Settings")]
        private Ship _target;

        public void Initialize(Enemy enemy, Ship target)
        {
            _enemy = enemy;
            _target = target;
        }
        
        private void FixedUpdate()
        {
            if (_enemy == null)
                return;
            
            if (_enemy.HealthComponent.IsDead 
                || _target == null 
                || _target.HealthComponent.IsDead)
                return;

            if(_navigation.IsReached)
                _enemy.Fire();
        }
    }
}