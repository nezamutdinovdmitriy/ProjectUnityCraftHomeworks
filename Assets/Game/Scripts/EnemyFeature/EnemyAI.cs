using UnityEngine;

namespace Game
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField]
        private Enemy _enemy;
        [SerializeField]
        private float _stoppingDistance = 0.25f;

        [Header("AI Settings")]
        private Ship _target;
        private Vector2 _destination;

        public void SetTarget(Ship target) => _target = target;
        public void SetDestination(Vector2 destination) => _destination = destination;

        private void FixedUpdate()
        {
            if (_enemy.HealthComponent.IsDead || _target == null || _target.HealthComponent.IsDead)
                return;

            Vector2 currentPosition = _enemy.transform.position;
            Vector2 vectorToDestination = _destination - currentPosition;

            bool isNotReached = vectorToDestination.sqrMagnitude > _stoppingDistance * _stoppingDistance;

            _enemy.SetMoveDirection(isNotReached ? vectorToDestination.normalized : Vector3.zero);

            if (isNotReached)
                _enemy.SetMoveDirection(vectorToDestination.normalized);
            else
                _enemy.Fire();
        }
    }
}