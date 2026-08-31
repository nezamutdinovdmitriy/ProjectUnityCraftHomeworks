using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    public class MoveToTargetNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;

        [SerializeField]
        private float _stoppingDistance = 0.5f;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) == false
                || character.TryGetComponent(out MoveComponent moveComponent) == false
                || _blackboard.TryGetValue(BlackboardAPI.Target, out GameObject target) == false)
                return BehaviourResult.Failure;
            
            Vector3 selfPosition = character.transform.position;
            
            Vector3 vector = target.transform.position - selfPosition;
            vector.y = 0f;

            Vector3 direction = vector.normalized;

            float sqrDistance = vector.sqrMagnitude;

            float sqrStoppingDistance = _stoppingDistance * _stoppingDistance;
            
            if (sqrDistance <= sqrStoppingDistance)
                return BehaviourResult.Success;
            
            moveComponent.MoveStep(direction, deltaTime);
            return BehaviourResult.Running;
        }
    }
}