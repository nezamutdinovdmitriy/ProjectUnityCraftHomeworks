using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public class MoveRequestComponent : IFixedTickable
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

        private bool _isRequested;
        private Vector2 _moveDirection;

        private IAction _action;
        private ICondition _condition;

        public void SetAction(IAction action) => _action = action;
        public void SetCondition(ICondition condition) => _condition = condition;
        public void SetMoveDirection(Vector2 direction)
        {
            _moveDirection = direction;
            _isRequested = true;
        }

        public void FixedTick()
        {
            if (_isRequested 
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

            _isRequested = false;
        }
    }
}