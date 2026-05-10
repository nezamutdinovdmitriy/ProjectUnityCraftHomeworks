using UnityEngine;

namespace Game
{
    public class EnemyCombatAI : MonoBehaviour
    {
        [SerializeField]
        private Enemy _enemy;
        [SerializeField]
        private EnemyNavigationAI _navigation;
        
        [Header("AI Settings")]
        private Ship _target;

        public void SetTarget(Ship target) => _target = target;

        private void FixedUpdate()
        {
            if (_enemy.HealthComponent.IsDead 
                || _target == null 
                || _target.HealthComponent.IsDead)
                return;

            if(_navigation.IsReached)
                _enemy.Fire();
        }
    }
}