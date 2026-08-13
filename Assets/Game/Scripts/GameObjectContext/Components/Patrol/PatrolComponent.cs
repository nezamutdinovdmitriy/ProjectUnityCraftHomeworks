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

            [field: SerializeField]
            public float StoppingDistance { get; private set; }
        }

        private readonly Transform[] _patrolPoints;
        private readonly MoveRequestComponent _moveRequestComponent;
        private readonly Transform _transformSelf;
        private readonly float _stoppingDistance;

        private int _currentPointIndex = -1;

        private Vector3 CurrentPointPosition => _patrolPoints[_currentPointIndex].position;

        public PatrolComponent(
            Settings settings, 
            Transform transformSelf, 
            MoveRequestComponent moveRequestComponent)
        {
            _transformSelf = transformSelf;
            _moveRequestComponent = moveRequestComponent;
            _stoppingDistance = settings.StoppingDistance;
            _patrolPoints = settings.PatrolPoints;

            SwitchToNextPoint();
        }

        public void FixedTick()
        {
            bool isReached = MoveUseCase.IsReached(
                _transformSelf.position,
                CurrentPointPosition,
                _stoppingDistance);

            if (isReached)
                SwitchToNextPoint();

            Vector3 direction = MoveUseCase.GetDirection(_transformSelf.position, CurrentPointPosition);
            _moveRequestComponent.RequestMove(direction);
        }

        private void SwitchToNextPoint()
            => _currentPointIndex = (_currentPointIndex + 1) % _patrolPoints.Length;
    }
}