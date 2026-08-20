using System;
using UnityEngine;

namespace SampleGame
{
    public sealed class MoveComponent : MonoBehaviour
    {
        public interface ICondition
        {
            public bool IsMet(Vector3 direction);
        }

        public interface IAction
        {
            void Invoke(Vector3 direction, float deltaTime);
        }

        public event Action<Vector3> OnMoved;
        
        public bool IsMoving => Time.time <= _timestamp;
        
        private ICondition _condition;
        private IAction _action;

        [SerializeField]
        private float _duration = 0.04f;
        
        private float _timestamp;

        private void Awake()
        {
            _timestamp = Time.time - _duration;
        }

        public void SetCondition(ICondition condition)
        {
            _condition = condition;
        }

        public void SetAction(IAction action)
        {
            _action = action;
        }

        public bool CanMove(Vector3 direction)
        {
            return direction != Vector3.zero && (_condition == null || _condition.IsMet(direction));
        }

        public void MoveStep(Vector3 direction, float deltaTime)
        {
            if (this.CanMove(direction))
            {
                _action.Invoke(direction, deltaTime);
                _timestamp = Time.time + _duration;
                this.OnMoved?.Invoke(direction);
            }
        }
    }
}