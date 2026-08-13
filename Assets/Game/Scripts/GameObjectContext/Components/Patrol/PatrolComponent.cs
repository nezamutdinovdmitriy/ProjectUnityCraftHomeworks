using System;
using UnityEngine;
using Zenject;

namespace GameObjects.Components
{
    public class PatrolComponent : IFixedTickable
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public Transform[] PatrolPoints { get; private set; }
        }

        public interface ICondition
        {
            public bool Evaluate();
        }

        private readonly Transform[] _patrolPoints;
        private readonly TargetComponent _targetComponent;

        private ICondition _condition;

        private int _currentPointIndex = -1;

        public PatrolComponent(Settings settings, TargetComponent targetComponent)
        {
            _targetComponent = targetComponent;
            _patrolPoints = settings.PatrolPoints;

            SwitchToNextPoint();
        }

        public void SetCondition(ICondition condition) => _condition = condition;

        public void FixedTick()
        {
            if (_condition.Evaluate())
                SwitchToNextPoint();
        }

        private void SwitchToNextPoint()
        {
            _currentPointIndex = (_currentPointIndex + 1) % _patrolPoints.Length;

            Transform point = _patrolPoints[_currentPointIndex];
            _targetComponent.Target = point.gameObject;
        }
    }
}