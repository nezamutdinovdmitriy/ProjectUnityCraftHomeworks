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
            if (_blackboard.TryGetValue(BlackboardAPI.CurrentCommand, out ICommandData commandData) == false)
                return BehaviourResult.Failure;

            if (commandData is IHasCommandPoint commandPoint)
                return AssignPosition(commandPoint);

            if (commandData is PatrolCommandData patrolCommandData)
                return AssignPosition(patrolCommandData);

            return BehaviourResult.Failure;
        }

        private BehaviourResult AssignPosition(PatrolCommandData command)
        {
            if (command.Points == null || command.Points.Count == 0)
            {
                _blackboard.DelValue(BlackboardAPI.TargetPosition);
                return BehaviourResult.Failure;
            }

            if (_blackboard.TryGetValue(BlackboardAPI.PatrolPointIndex, out int index) == false)
                index = 0;

            while (command.Points.Count > 0)
            {
                index %= command.Points.Count;
                CommandPoint point = command.Points[index];

                if (TryGetPosition(point, out Vector3 position))
                {
                    _blackboard.SetPrimitiveValue(BlackboardAPI.TargetPosition, position);
                    _blackboard.SetPrimitiveValue(BlackboardAPI.PatrolPointIndex, index);
                    return BehaviourResult.Success;
                }
                
                command.Points.RemoveAt(index);
            }

            _blackboard.DelValue(BlackboardAPI.TargetPosition);
            return BehaviourResult.Failure;
        }

        private BehaviourResult AssignPosition(IHasCommandPoint command)
        {
            if (TryGetPosition(command.Point, out Vector3 position))
            {
                _blackboard.SetPrimitiveValue(BlackboardAPI.TargetPosition, position);
                return BehaviourResult.Success;
            }

            _blackboard.DelValue(BlackboardAPI.TargetPosition);
            return BehaviourResult.Failure;
        }
        
        private bool TryGetPosition(CommandPoint point, out Vector3 position)
        {
            if (point.Position.HasValue)
            {
                position = point.Position.Value;
                return true;
            }
            
            if (point.Target != null)
            {
                position = point.Target.transform.position;
                return true;
            }

            position = default;
            return false;
        }
    }
}