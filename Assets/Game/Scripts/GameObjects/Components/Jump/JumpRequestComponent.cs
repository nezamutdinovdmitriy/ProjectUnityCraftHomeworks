using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public class JumpRequestComponent : IFixedTickable
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public float Cooldown { get; private set; } = 0.25f;

            [field: SerializeField]
            public float Delay { get; private set; } = 0.05f;
        }
        
        public interface IAction
        {
            public void Invoke();
        }
        
        public interface ICondition
        {
            public bool Evaluate();
        }

        public event Action Jumped;

        private readonly Settings _settings;
        
        private IAction _action;
        private ICondition _condition;

        private float _requestTime;
        private float _nextAllowedJumpTime;
        
        private bool _isRequested;

        public JumpRequestComponent(Settings settings) => _settings = settings;

        public void SetAction(IAction action) => _action = action;
        public void SetCondition(ICondition condition) => _condition = condition;

        public void RequestJump()
        {
            if (Time.time < _nextAllowedJumpTime)
                return;

            _requestTime = Time.time;
            _isRequested = true;
        }

        public void FixedTick()
        {
            bool canJump = _condition == null || _condition.Evaluate();
            
            if (_isRequested == false
                || Time.time < _requestTime + _settings.Delay)
                return;

            if (canJump == false)
            {
                _isRequested = false;
                return;
            }

            _action?.Invoke();
            Jumped?.Invoke();

            _nextAllowedJumpTime = Time.time + _settings.Cooldown;

            _isRequested = false;
        }
    }
}