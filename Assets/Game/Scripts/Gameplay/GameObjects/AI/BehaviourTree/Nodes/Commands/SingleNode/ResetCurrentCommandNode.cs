using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    public class ResetCurrentCommandNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.HasValue(BlackboardAPI.CurrentCommand) == false)
                return BehaviourResult.Failure;
            
            _blackboard.DelValue(BlackboardAPI.CurrentCommand);
            return BehaviourResult.Success;
        }
    }
}