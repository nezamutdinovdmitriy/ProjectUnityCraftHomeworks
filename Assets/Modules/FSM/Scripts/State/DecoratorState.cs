using Modules.AI;
using UnityEngine;
using UnityEngine.Events;

namespace Modules.FSM
{
    public sealed class DecoratorState : State
    {
        [Header("Decorator")]
        [SerializeField]
        private State _origin;

        [SerializeField]
        private UnityEvent _enterEvent;
        
        [SerializeField]
        private UnityEvent _exitEvent;
        
        [SerializeReference]
        private IAction _onEnter;
        
        [SerializeReference]
        private IAction _onExit;

        public override void OnEnter()
        {
            _enterEvent.Invoke();
            _onEnter?.Invoke();
            _origin.OnEnter();
        }

        public override void OnUpdate(float deltaTime)
        {
            _origin.OnUpdate(deltaTime);
        }

        public override void OnExit()
        {
            _exitEvent.Invoke();
            _origin.OnExit();
            _onExit?.Invoke();
        }
    }
}