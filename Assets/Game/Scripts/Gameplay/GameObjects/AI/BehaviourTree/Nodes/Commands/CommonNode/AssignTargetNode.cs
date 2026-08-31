using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    public class AssignTargetNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.TryGetValue(BlackboardAPI.CurrentCommand, out ICommandData commandData) == false
                || commandData is not IHasCommandPoint commandPoint)
                return BehaviourResult.Failure;

            if (commandPoint.Point.Target != null)
            {
                _blackboard.SetReferenceValue(BlackboardAPI.Target, commandPoint.Point.Target);
                return BehaviourResult.Success;
            }

            return BehaviourResult.Failure;
        }
    }
}