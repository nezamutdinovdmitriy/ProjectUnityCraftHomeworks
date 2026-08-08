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

        private readonly Transform[] _patrolPoints;
        private readonly TargetComponent _targetComponent;

        private ICondition _condition;
        private IAction _action;

        private int _currentPointIndex = -1;

        public PatrolComponent(Settings settings, TargetComponent targetComponent)
        {
            _targetComponent = targetComponent;
            _patrolPoints = settings.PatrolPoints.ToArray();

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
            _currentPointIndex = (_currentPointIndex + 1) % _patrolPoints.Length;

            Transform point = _patrolPoints[_currentPointIndex];

            _targetComponent.Target = point.gameObject;
        }
    }
}