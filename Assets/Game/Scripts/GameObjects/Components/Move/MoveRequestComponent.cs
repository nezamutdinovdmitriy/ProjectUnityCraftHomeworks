using System;
using UnityEngine;

namespace Game
{
    public class MoveRequestComponent : MonoBehaviour
    {
        public interface IAction
        {
            public void Invoke(Vector2 direction);
        }

        public interface ICondition
        {
            public bool Evaluate();
        }

        public event Action<Vector2> Moved;

        private bool _moveRequired;
        private Vector2 _moveDirection;

        private IAction _action;
        private ICondition _condition;

        public void SetAction(IAction action) => _action = action;
        public void SetCondition(ICondition condition) => _condition = condition;
        public void SetMoveDirection(Vector2 direction)
        {
            _moveDirection = direction;
            _moveRequired = true;
        }

        private void FixedUpdate()
        {
            if (_moveRequired 
                && _moveDirection != Vector2.zero
                && (_condition == null || _condition.Evaluate()))
            {
                _action?.Invoke(_moveDirection);
                Moved?.Invoke(_moveDirection);
            }
            else
            {
                Moved?.Invoke(Vector2.zero);
            }

            _moveRequired = false;
        }
    }
}