using Modules.AI;
using UnityEngine;

namespace SampleGame.AI.BehaviourTree.Conditions
{
    public class HasNearestEnemy : ICondition
    {
        [SerializeField] private Blackboard _blackboard;
        
        public bool Invoke()
        {
            if (_blackboard.HasValue(BlackboardAPI.NearestEnemy))
                return true;

            return false;
        }
    }
}