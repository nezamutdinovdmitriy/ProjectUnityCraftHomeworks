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
        
        private bool _requested;
        
        public void SetAction(IAction action) => _action = action;
        public void SetCondition(ICondition condition) => _condition = condition;

        public void RequestDeath()
        {
            _requestTime = Time.time + _delay;
            _requested = true;
        }
        
        public void FixedUpdate()
        {
            bool canDie = _condition == null || _condition.Evaluate();

            if (_requested == false || Time.fixedTime < _requestTime )
                return;

            if (canDie == false)
            {
                _requested = false;
                return;
            }
            
            _action?.Invoke();
            _requested = false;
        }
    }
}