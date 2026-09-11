using Modules.AI;
using UnityEngine;

namespace SampleGame.AI.BehaviourTree.Conditions
{
    public class IsCommandQueueNotEmpty : ICondition
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        public bool Invoke()
        {
            if (_blackboard.TryGetValue(BlackboardAPI.CommandQueue, out var queue)
                && queue.Count > 0)
                return true;

            return false;
        }
    }
}