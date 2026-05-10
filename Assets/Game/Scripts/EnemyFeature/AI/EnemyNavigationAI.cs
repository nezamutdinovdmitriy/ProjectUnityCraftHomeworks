using UnityEngine;

namespace Game
{
    public class EnemyNavigationAI : MonoBehaviour
    {
        [SerializeField]
        private Enemy _enemy;
        [SerializeField]
        private float _stoppingDistance = 0.25f;
        
        [Header("AI Settings")]
        private Vector2 _destination;
        
        public bool IsReached { get; private set; }
        
        public void SetDestination(Vector2 destination) => _destination = destination;
        
        private void FixedUpdate()
        {
            if (_enemy.HealthComponent.IsDead)
                return;

            Vector2 vectorToDestination = _destination - (Vector2) _enemy.transform.position;
            float sqrDistance = vectorToDestination.sqrMagnitude;

            IsReached = sqrDistance <= _stoppingDistance * _stoppingDistance;

            _enemy.SetMoveDirection(IsReached ? Vector3.zero : vectorToDestination.normalized);
        }
    }
}