using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    public class AssignPatrolPointNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.TryGetValue(BlackboardAPI.PatrolPoints, out List<CommandPoint> points) == false
                || _blackboard.TryGetValue(BlackboardAPI.PatrolPointIndex, out int index) == false
                || points == null
                || index < 0
                || index >= points.Count)
                return BehaviourResult.Failure;

            CommandPoint point = points[index];
            
            _blackboard.DelValue(BlackboardAPI.Target);
            _blackboard.DelValue(BlackboardAPI.TargetPosition);

            if (point.Target != null)
            {
                _blackboard.SetReferenceValue(
                    BlackboardAPI.Target,
                    point.Target);

                return BehaviourResult.Success;
            }

            if (point.Position.HasValue)
            {
                _blackboard.SetPrimitiveValue(
                    BlackboardAPI.TargetPosition,
                    point.Position.Value);

                return BehaviourResult.Success;
            }

            return BehaviourResult.Failure;
        }
    }
}