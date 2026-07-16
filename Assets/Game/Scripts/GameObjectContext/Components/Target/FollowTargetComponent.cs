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
        private readonly TargetComponent _targetComponent;

        public FollowTargetComponent(
            Settings settings, 
            Transform transform, 
            TargetComponent targetComponent)
        {
            _settings = settings;
            _transform = transform;
            _targetComponent = targetComponent;
        }
        
        public bool TryGetFollowDirection(out Vector2 direction)
        {
            if (_targetComponent.Target == null || IsDestinationReached())
            {
                direction = Vector2.zero;
                return false;
            }

            direction = GetDirectionToTarget();
            return true;
        }
        
        public bool IsDestinationReached() => GetDistanceToTarget() <= _settings.StoppingDistance;

        private Vector2 GetDirectionToTarget() 
            => ((Vector2) _targetComponent.Target.transform.position - (Vector2) _transform.position).normalized;

        private float GetDistanceToTarget() 
            => ((Vector2) _targetComponent.Target.transform.position - (Vector2) _transform.position).magnitude;
    }
}