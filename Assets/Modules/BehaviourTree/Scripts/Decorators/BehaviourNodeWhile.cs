using UnityEngine;

namespace Modules.AI
{
    [AddComponentMenu("AI/BehaviourTree/BehaviourNode «While»")]
    public sealed class BehaviourNodeWhile : BehaviourNode, IBehaviourNodeDecorator
    {
        public BehaviourNode Child => _child;

        [Space]
        [SerializeReference]
        private ICondition _condition;

        [SerializeField]
        private BehaviourNode _child;

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_condition.Invoke())
            {
                _child.Run(deltaTime);
                return BehaviourResult.Running;
            }

            return BehaviourResult.Failure;
        }

        protected override void OnStop(BehaviourResult result)
        {
            if (_child.IsRunning)
                _child.Abort();
        }
    }
}