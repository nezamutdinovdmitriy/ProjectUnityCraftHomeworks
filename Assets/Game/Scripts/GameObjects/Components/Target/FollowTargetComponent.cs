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
        private readonly Transform _transform;
        
        private Vector2 _targetPoint;

        public FollowTargetComponent(Settings settings, Transform transform)
        {
            _settings = settings;
            _transform = transform;
        }

        public void SetTargetPoint(Vector2 target) => _targetPoint = target;

        public bool IsDestinationReached() => GetDistanceToTarget() <= _settings.StoppingDistance;

        public Vector2 GetDirectionToTarget() 
            => (_targetPoint - (Vector2) _transform.position).normalized;

        private float GetDistanceToTarget() 
            => (_targetPoint - (Vector2) _transform.position).magnitude;
    }
}