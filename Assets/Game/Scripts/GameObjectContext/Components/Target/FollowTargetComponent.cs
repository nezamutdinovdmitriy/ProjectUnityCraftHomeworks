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

        public interface ICondition
        {
            public bool Evaluate();
        }
        
        private readonly Settings _settings;
        private readonly Transform _transform;
        private readonly TargetComponent _targetComponent;
        private readonly MoveRequestComponent _moveRequestComponent;

        private ICondition _condition;
        
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

        public void SetCondition(ICondition condition) 
            => _condition = condition;
        
        public void FixedTick()
        {
            if (_condition.Evaluate() == false)
                return;
            
            bool IsReached = MoveUseCase.IsReached(
                _targetComponent.Target.transform.position,
                _transform.position,
                _settings.StoppingDistance);
            
            if (IsReached)
                return;
            
            Vector2 directionToTarget = MoveUseCase.GetDirection(
                _transform.position,
                _targetComponent.Target.transform.position);
            
            _moveRequestComponent.RequestMove(directionToTarget);   
        }
    }
}