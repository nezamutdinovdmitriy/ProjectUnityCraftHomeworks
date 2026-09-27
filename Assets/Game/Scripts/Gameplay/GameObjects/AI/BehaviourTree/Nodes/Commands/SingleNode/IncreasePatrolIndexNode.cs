using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    public class IncreasePatrolIndexNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.TryGetValue(BlackboardAPI.PatrolPointIndex, out int index) == false
                || _blackboard.TryGetValue(BlackboardAPI.PatrolPoints, out List<CommandPoint> points) == false
                || points.Count == 0)
                return BehaviourResult.Failure;
            
            int nextIndex = (index + 1) % points.Count;

            _blackboard.SetPrimitiveValue(BlackboardAPI.PatrolPointIndex, nextIndex);

            return BehaviourResult.Success;
        }
    }
}