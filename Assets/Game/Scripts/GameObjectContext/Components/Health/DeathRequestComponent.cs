using System;
using UnityEngine;
using Zenject;

namespace GameObjects.Components
{
    public class DeathRequestComponent : IFixedTickable
    {
        [Serializable]
        public class Settings
        {
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

        private readonly Settings _settings;
        
        private IAction _action;
        private ICondition _condition;

        private float _requestTime;
        private bool _isRequested;

        public DeathRequestComponent(Settings settings) => _settings = settings;

        public void SetAction(IAction action) => _action = action;
        public void SetCondition(ICondition condition) => _condition = condition;

        public void RequestDeath()
        {
            _requestTime = Time.time + _settings.Delay;
            _isRequested = true;
        }

        public void FixedTick()
        {
            bool canDie = _condition == null || _condition.Evaluate();

            if (_isRequested == false || Time.fixedTime < _requestTime )
                return;

            if (canDie == false)
            {
                _isRequested = false;
                return;
            }
            
            _action?.Invoke();
            _isRequested = false;
        }
    }
}