using UnityEngine;

namespace Game.Target
{
    public class FollowTargetComponent : MonoBehaviour
    {
        [SerializeField]
        private float _stoppingDistance;

        private Vector2 _targetPoint;

        public void SetTargetPoint(Vector2 target) => _targetPoint = target;

        public bool IsDestinationReached() => GetDistanceToTarget() <= _stoppingDistance;

        public Vector2 GetDirectionToTarget() 
            => (_targetPoint - (Vector2) transform.position).normalized;

        private float GetDistanceToTarget() 
            => (_targetPoint - (Vector2) transform.position).magnitude;
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.rebeccaPurple;

            Gizmos.DrawWireSphere(transform.position, _stoppingDistance);
        }
    }
}