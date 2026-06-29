using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public class AttackRequestComponent : IFixedTickable
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public float Cooldown { get; private set; }

            [field: SerializeField]
            public float Delay { get; private set; }
        }
        
        public interface IAction
        {
            public void Invoke();
        }
        
        public interface ICondition
        {
            public bool Evaluate();
        }

        public event Action Attacked;

        private readonly Settings _settings;

        private bool _isRequested;
        private float _requestTime;
        private float _nextAllowedAttackTime;
        
        private IAction _action;
        private ICondition _condition;

        public AttackRequestComponent(Settings settings) => _settings = settings;

        public void SetAction(IAction action) => _action = action;
        public void SetCondition(ICondition condition) => _condition = condition;

        public bool IsRequested => _isRequested;
        
        public void RequestAttack()
        {
            if (Time.time < _nextAllowedAttackTime)
                return;

            _requestTime = Time.time;
            _isRequested = true;
        }

        public void FixedTick()
        {
            bool canAttack = _condition == null || _condition.Evaluate();

            if (_isRequested == false || Time.time < _requestTime + _settings.Delay)
                return;

            if (canAttack == false)
            {
                _isRequested = false;
                return;
            }
            
            _action?.Invoke();
            Attacked?.Invoke();

            _nextAllowedAttackTime = Time.time + _settings.Cooldown;
            
            _isRequested = false;
        }
    }
}