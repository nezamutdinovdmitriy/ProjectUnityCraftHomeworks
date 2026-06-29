using UnityEngine;

namespace Game
{
    public class DeathComponent : MonoBehaviour
    {
        public interface IAction
        {
            public void Invoke();
        }
        
        public interface ICondition
        {
            public bool Evaluate();
        }
        
        private IAction _action;
        private ICondition _condition;

        private float _requestTime;
        
        [SerializeField]
        private float _delay;
        
        private bool _isRequested;
        
        public void SetAction(IAction action) => _action = action;
        public void SetCondition(ICondition condition) => _condition = condition;

        public void RequestDeath()
        {
            _requestTime = Time.time + _delay;
            _isRequested = true;
        }
        
        public void FixedUpdate()
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