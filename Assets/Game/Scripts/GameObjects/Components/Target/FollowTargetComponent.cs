using System;
using UnityEngine;

namespace Game.Target
{
    public class FollowTargetComponent
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public float StoppingDistance { get; private set; }
        }

        private readonly Settings _settings;
        private readonly TransformComponent _transform;
        
        private Vector2 _targetPoint;

        public FollowTargetComponent(
            Settings settings, 
            TransformComponent transform)
        {
            _settings = settings;
            _transform = transform;
        }

        public void SetTargetPoint(Vector2 target) => _targetPoint = target;

        public bool IsDestinationReached() => GetDistanceToTarget() <= _settings.StoppingDistance;

        public Vector2 GetDirectionToTarget() 
            => (_targetPoint - (Vector2) _transform.Position).normalized;

        private float GetDistanceToTarget() 
            => (_targetPoint - (Vector2) _transform.Position).magnitude;
        
        // private void OnDrawGizmosSelected()
        // {
        //     Gizmos.color = Color.rebeccaPurple;
        //
        //     Gizmos.DrawWireSphere(transform.position, _stoppingDistance);
        // }
    }
}