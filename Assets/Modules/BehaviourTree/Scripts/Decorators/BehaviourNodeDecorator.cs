using UnityEngine;
using UnityEngine.Events;

namespace Modules.AI
{
    [AddComponentMenu("AI/BehaviourTree/BehaviourNode «Decorator»")]
    public class BehaviourNodeDecorator : BehaviourNode, IBehaviourNodeDecorator
    {
        public BehaviourNode Child => _origin;
        
        [Space]
        [SerializeField]
        private BehaviourNode _origin;
        
        [Header("Events")]
        [SerializeField]
        private UnityEvent _startEvent;

        [SerializeField]
        private UnityEvent _stopEvent;

        [SerializeField]
        private UnityEvent _updateEvent;

        [SerializeField]
        private UnityEvent _abortEvent;
        
        [Header("Actions")]
        [SerializeReference]
        private IAction _onStart;

        [SerializeReference]
        private IAction _onStop;
        
        [SerializeReference]
        private IAction _onUpdate;
        
        [SerializeReference]
        private IAction _onAbort;
        
        protected override void OnStart()
        {
            _startEvent.Invoke();
            _onStart?.Invoke();
        }

        protected override void OnStop(BehaviourResult _)
        {
            _stopEvent.Invoke();
            _onStop?.Invoke();
        }

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            _updateEvent.Invoke();
            _onUpdate?.Invoke();
            return _origin.Run(deltaTime);
        }

        protected override void OnAbort()
        {
            _onAbort?.Invoke();
            _origin.Abort();
        }
    }
}