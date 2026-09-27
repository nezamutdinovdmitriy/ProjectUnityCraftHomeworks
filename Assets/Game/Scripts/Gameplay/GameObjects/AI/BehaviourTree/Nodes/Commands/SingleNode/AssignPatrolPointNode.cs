using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    public class AssignPatrolPointNode : BehaviourNode
    {
        [SerializeField] private Blackboard _blackboard;

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.TryGetValue(BlackboardAPI.PatrolPoints, out List<CommandPoint> points) == false
                || _blackboard.TryGetValue(BlackboardAPI.PatrolPointIndex, out int index) == false
                || points == null
                || points.Count == 0)
                return BehaviourResult.Failure;

            index %= points.Count;

            while (points.Count > 0)
            {
                CommandPoint point = points[index];

                if (point.Target != null || point.Position.HasValue)
                {
                    _blackboard.SetPrimitiveValue(BlackboardAPI.PatrolPointIndex, index);

                    _blackboard.DelValue(BlackboardAPI.Target);
                    _blackboard.DelValue(BlackboardAPI.TargetPosition);

                    SetPatrolPoint(point);

                    return BehaviourResult.Success;
                }

                points.RemoveAt(index);

                if (points.Count == 0)
                    return BehaviourResult.Failure;

                if (index >= points.Count)
                    index = 0;
            }

            return BehaviourResult.Failure;
        }

        private void SetPatrolPoint(CommandPoint point)
        {
            if (point.Target != null)
            {
                _blackboard.SetReferenceValue(
                    BlackboardAPI.Target,
                    point.Target);
            }
            else if (point.Position != null)
            {
                _blackboard.SetPrimitiveValue(
                    BlackboardAPI.TargetPosition,
                    point.Position.Value);
            }
        }
    }
}