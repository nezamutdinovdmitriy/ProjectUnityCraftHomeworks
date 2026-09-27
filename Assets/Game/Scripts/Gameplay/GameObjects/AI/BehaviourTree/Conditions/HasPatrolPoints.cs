using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace SampleGame.AI.BehaviourTree.Conditions
{
    public class HasPatrolPoints : ICondition
    {
        [SerializeField] private Blackboard _blackboard;
        
        public bool Invoke()
        {
            return _blackboard.TryGetValue(BlackboardAPI.PatrolPoints, out List<CommandPoint> points)
                   && points is {Count: > 0};
        }
    }
}