using Modules.AI;
using UnityEngine;

namespace SampleGame.AI.BehaviourTree.Conditions
{
    public class TargetIsReachedCondition : ICondition
    {
        [SerializeField]
        private Blackboard _blackboard;

        [SerializeField] [BlackboardValueKey(typeof(float))]
        private string _stoppingDistanceKey;
        
        public bool Invoke()
        {
            if (_blackboard.TryGetValue(BlackboardAPI.TargetPosition, out Vector3 targetPosition) == false
                || _blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) == false
                || _blackboard.TryGetValue(_stoppingDistanceKey, out float stoppingDistance) == false)
                return false;

            Vector3 selfPosition = character.transform.position;

            Vector3 vector = targetPosition - selfPosition;
            float sqrDistance = stoppingDistance * stoppingDistance;
            
            bool isReached = vector.sqrMagnitude <= sqrDistance;

            if (isReached)
                return true;
            
            return false;
        }
    }
}