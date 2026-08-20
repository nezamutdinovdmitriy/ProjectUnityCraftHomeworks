using UnityEngine;
using UnityEngine.Events;

namespace Modules.AI
{
    [AddComponentMenu("AI/BehaviourTree/BehaviourNode «Action»")]
    public sealed class BehaviourNodeAction : BehaviourNode
    {
        [Space]
        [SerializeField]
        private UnityEvent _event;

        [SerializeReference]
        private IAction _action;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            _event.Invoke();
            _action?.Invoke();
            return BehaviourResult.Success;
        }
    }
}