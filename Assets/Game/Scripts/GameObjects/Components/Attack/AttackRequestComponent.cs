using System;
using UnityEngine;

namespace Game
{
    public class AttackRequestComponent : MonoBehaviour
    {
        public interface IAction
        {
            public void Invoke();
        }
        
        public interface ICondition
        {
            public bool Evaluate();
        }

        public event Action Attacked;
        
        [SerializeField]
        private float _cooldown;

        [SerializeField]
        private float _delay;

        private bool _isRequested;
        private float _requestTime;
        private float _nextAllowedAttackTime;
        
        private IAction _action;
        private ICondition _condition;
        
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

        public void FixedUpdate()
        {
            bool canAttack = _condition == null || _condition.Evaluate();

            if (_isRequested == false || Time.time < _requestTime + _delay)
                return;

            if (canAttack == false)
            {
                _isRequested = false;
                return;
            }
            
            _action?.Invoke();
            Attacked?.Invoke();

            _nextAllowedAttackTime = Time.time + _cooldown;
            
            _isRequested = false;
        }
    }
}