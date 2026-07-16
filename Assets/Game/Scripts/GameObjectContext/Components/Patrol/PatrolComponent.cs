using System;
using System.Collections.Generic;
using Game.Target;
using UnityEngine;
using Zenject;

namespace Game.Patrol
{
    public class PatrolComponent : IFixedTickable
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public List<Transform> PatrolPoints { get; private set; }
        }
        
        public interface ICondition
        {
            public bool Evaluate();
        }
        
        public interface IAction
        {
            public void Invoke();
        }
        
        private readonly Queue<Transform> _patrolPointsQueue = new();
        private readonly TargetComponent _targetComponent;

        private ICondition _condition;
        private IAction _action;

        public PatrolComponent(Settings settings, TargetComponent targetComponent)
        {
            _targetComponent = targetComponent;
            
            foreach (Transform patrolPoint in settings.PatrolPoints)
                _patrolPointsQueue.Enqueue(patrolPoint);
            
            SwitchToNextPoint();
        }

        public void SetAction(IAction action) => _action = action;
        public void SetCondition(ICondition condition) => _condition = condition;

        public void FixedTick()
        {
            if (_condition.Evaluate())
            {
                SwitchToNextPoint();
                _action?.Invoke();
            }
        }

        private void SwitchToNextPoint()
        {
            Transform point = _patrolPointsQueue.Dequeue();
            _patrolPointsQueue.Enqueue(point);
            
            _targetComponent.Target = point.gameObject;
        }
    }
}