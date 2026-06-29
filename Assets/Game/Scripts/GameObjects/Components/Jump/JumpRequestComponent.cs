using System;
using UnityEngine;

namespace Game
{
    public class JumpRequestComponent : MonoBehaviour
    {
        public interface IAction
        {
            public void Invoke();
        }
        
        public interface ICondition
        {
            public bool Evaluate();
        }

        public event Action Jumped;

        [SerializeField]
        private float _jumpCooldown = 0.25f;

        [SerializeField]
        private float _jumpDelay = 0.05f;
        
        private IAction _action;
        private ICondition _condition;

        private float _requestTime;
        private float _nextAllowedJumpTime;
        
        private bool _isRequested;

        public void SetAction(IAction action) => _action = action;
        public void SetCondition(ICondition condition) => _condition = condition;

        public void RequestJump()
        {
            if (Time.time < _nextAllowedJumpTime)
                return;

            _requestTime = Time.time;
            _isRequested = true;
        }

        private void FixedUpdate()
        {
            bool canJump = _condition == null || _condition.Evaluate();
            
            if (_isRequested == false
                || Time.time < _requestTime + _jumpDelay)
                return;

            if (canJump == false)
            {
                _isRequested = false;
                return;
            }

            _action?.Invoke();
            Jumped?.Invoke();

            _nextAllowedJumpTime = Time.time + _jumpCooldown;

            _isRequested = false;
        }
    }
}