using Modules.AI;
using UnityEngine;

namespace SampleGame.AI.BehaviourTree.Conditions
{
    public class TargetIsReachedCondition : ICondition
    {
        [SerializeField]
        private Blackboard _blackboard;

        [SerializeField]
        private float _stoppingDistance;
        
        public bool Invoke()
        {
            if (_blackboard.TryGetValue(BlackboardAPI.TargetPosition, out Vector3 targetPosition) == false
                || _blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) == false)
                return false;
            
            Vector3 selfPosition = character.transform.position;

            Vector3 vector = targetPosition - selfPosition;
            float sqrDistance = _stoppingDistance * _stoppingDistance;
            
            bool isReached = vector.sqrMagnitude <= sqrDistance;

            if (isReached)
                return true;
            
            return false;
        }
    }
}