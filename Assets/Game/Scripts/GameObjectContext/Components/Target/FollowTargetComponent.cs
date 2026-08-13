using System;
using UnityEngine;
using Zenject;

namespace GameObjects.Components
{
    public class FollowTargetComponent : IFixedTickable
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
        private readonly MoveRequestComponent _moveRequestComponent;

        public FollowTargetComponent(
            Settings settings, 
            Transform transform, 
            TargetComponent targetComponent, 
            MoveRequestComponent moveRequestComponent)
        {
            _settings = settings;
            _transform = transform;
            _targetComponent = targetComponent;
            _moveRequestComponent = moveRequestComponent;
        }
        
        public void FixedTick()
        {
            if (_targetComponent.Target == null || IsDestinationReached())
                return;
            
            Vector2 directionToTarget = GetDirectionToTarget();
            _moveRequestComponent.RequestMove(directionToTarget);   
        }
        
        public bool IsDestinationReached() => GetDistanceToTarget() <= _settings.StoppingDistance;

        private Vector2 GetDirectionToTarget() 
            => ((Vector2) _targetComponent.Target.transform.position - (Vector2) _transform.position).normalized;

        private float GetDistanceToTarget() 
            => ((Vector2) _targetComponent.Target.transform.position - (Vector2) _transform.position).magnitude;
    }
}