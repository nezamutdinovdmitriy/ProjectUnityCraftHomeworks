using UnityEngine;

namespace Modules.AI
{
    [AddComponentMenu("AI/BehaviourTree/BehaviourNode «Override»")]
    public sealed class BehaviourNodeOverride : BehaviourNode, IBehaviourNodeDecorator
    {
        public BehaviourNode Child => _origin;
        
        private enum SuccessPolicy
        {
            Origin = 0,
            Failure = 1,
            Running = 2
        }

        private enum FailurePolicy
        {
            Origin = 0,
            Success = 1,
            Running = 2
        }

        [Space]
        [SerializeField]
        private BehaviourNode _origin;

        [Header("Policy")]
        [SerializeField]
        private SuccessPolicy _successPolicy = SuccessPolicy.Origin;

        [SerializeField]
        private FailurePolicy _failurePolicy = FailurePolicy.Origin;

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            BehaviourResult result = _origin.Run(deltaTime);

            if (result == BehaviourResult.Failure)
            {
                if (_failurePolicy == FailurePolicy.Origin)
                    return result;
                if (_failurePolicy == FailurePolicy.Success)
                    return BehaviourResult.Success;
                if (_failurePolicy == FailurePolicy.Running)
                    return BehaviourResult.Running;
            }
            else if (result == BehaviourResult.Success)
            {
                if (_successPolicy == SuccessPolicy.Origin)
                    return result;
                if (_successPolicy == SuccessPolicy.Failure)
                    return BehaviourResult.Failure;
                if (_successPolicy == SuccessPolicy.Running)
                    return BehaviourResult.Running;
            }

            return result;
        }
    }
}