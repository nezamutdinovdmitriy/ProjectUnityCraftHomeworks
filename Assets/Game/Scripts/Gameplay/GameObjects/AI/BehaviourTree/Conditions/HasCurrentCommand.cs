using Modules.AI;
using UnityEngine;

namespace SampleGame.AI.BehaviourTree.Conditions
{
    public class HasCurrentCommand : ICondition
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        public bool Invoke()
        {
            if (_blackboard.TryGetValue(BlackboardAPI.CurrentCommand, out ICommandData command)
                && command.Type != CommandType.Default)
                return true;

            return false;
        }
    }
}