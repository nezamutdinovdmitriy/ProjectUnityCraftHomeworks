using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    public class AssignTargetPositionNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.HasValue(BlackboardAPI.TargetPosition) == false
                || _blackboard.TryGetValue(BlackboardAPI.CurrentCommand, out ICommandData command) == false
                || command is not IHasCommandPoint validCommand)
                return BehaviourResult.Failure;
            
            return AssignPosition(validCommand);
        }

        private BehaviourResult AssignPosition(IHasCommandPoint command)
        {
            if (command.Point.Position.HasValue)
            { 
                _blackboard.SetPrimitiveValue(
                    BlackboardAPI.TargetPosition,
                    command.Point.Position.Value);
                
                return BehaviourResult.Success;
            }
            
            if (command.Point.Target != null)
            {
                _blackboard.SetPrimitiveValue(
                    BlackboardAPI.TargetPosition,
                    command.Point.Target.transform.position);
                
                return BehaviourResult.Success;
            }

            return BehaviourResult.Failure;
        }
    }
}