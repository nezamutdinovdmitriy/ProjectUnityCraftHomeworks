using UnityEngine;

namespace Game
{
    public class EnemyNavigationAI : MonoBehaviour
    {
        /*[SerializeField]
        private float _stoppingDistance = 0.25f;
        
        private Enemy _enemy;
        
        [Header("AI Settings")]
        private Vector2 _destination;
        
        public bool IsReached { get; private set; }

        public void Initialize(Enemy enemy, Vector2 destination)
        {
            _enemy = enemy;
            _destination = destination;
        }
        
        private void FixedUpdate()
        {
            if (_enemy == null)
                return;
            
            if (_enemy.HealthComponent.IsDead)
                return;

            Vector2 vectorToDestination = _destination - (Vector2) _enemy.transform.position;
            float sqrDistance = vectorToDestination.sqrMagnitude;

            IsReached = sqrDistance <= _stoppingDistance * _stoppingDistance;

            _enemy.SetMoveDirection(IsReached ? Vector3.zero : vectorToDestination.normalized);
        }*/
    }
}