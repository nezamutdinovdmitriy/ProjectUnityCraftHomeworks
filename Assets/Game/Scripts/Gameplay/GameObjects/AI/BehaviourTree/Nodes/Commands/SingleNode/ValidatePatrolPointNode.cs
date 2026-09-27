using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    public class ValidatePatrolPointNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.TryGetValue(BlackboardAPI.PatrolPoints, out List<CommandPoint> points) == false
                || points == null
                || points.Count == 0 
                || _blackboard.TryGetValue(BlackboardAPI.PatrolPointIndex, out int index) == false)
                return BehaviourResult.Failure;
            
            index %= points.Count;

            while (points.Count > 0)
            {
                CommandPoint point = points[index];
                
                if (point.Position.HasValue || point.Target != null)
                {
                    _blackboard.SetPrimitiveValue(
                        BlackboardAPI.PatrolPointIndex,
                        index);

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
    }
}