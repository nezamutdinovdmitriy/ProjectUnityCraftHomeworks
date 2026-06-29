using System.Collections.Generic;
using UnityEngine;

namespace Game.Patrol
{
    public class PointProviderComponent : MonoBehaviour
    {
        public interface ICondition
        {
            public bool Evaluate();
        }
        
        public interface IAction
        {
            public void Invoke();
        }
        
        [SerializeField]
        private List<Transform> _patrolPoints;
        
        private readonly Queue<Transform> _patrolPointsQueue = new();
        
        private Vector2 _point;

        private ICondition _condition;
        private IAction _action;
        
        private void Awake()
        {
            foreach (Transform patrolPoint in _patrolPoints)
                _patrolPointsQueue.Enqueue(patrolPoint);

            SwitchToNextPoint();
        }

        public void SetAction(IAction action) => _action = action;
        
        public void SetCondition(ICondition condition) => _condition = condition;
        
        public Vector2 GetPoint() => _point;

        private void FixedUpdate()
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
            
            _point = point.position;
        }
    }
}