using Modules.AI;
using UnityEngine;

namespace SampleGame.AI.BehaviourTree.Conditions
{
    public class HasTargetCondition : ICondition
    {
        [SerializeField]
        private Blackboard _blackboard;

        public bool Invoke()
        {
            if (_blackboard.HasValue(BlackboardAPI.Target))
                return true;

            return false;
        }
    }
}