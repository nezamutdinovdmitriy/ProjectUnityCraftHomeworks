using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    public class AttackTargetNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;

        [SerializeField]
        private float _attackDistance;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) == false
                || _blackboard.TryGetValue(BlackboardAPI.Target, out GameObject target) == false
                || target == null
                || target.TryGetComponent(out HealthComponent healthComponent) == false
                || character.TryGetComponent(out AttackComponent attackComponent) == false)
                return BehaviourResult.Failure;

            Vector3 selfPosition = character.transform.position;
            Vector3 targetPosition = target.transform.position;

            Vector3 vector = targetPosition - selfPosition;
            vector.y = 0f;
            
            float sqrAttackDistance = _attackDistance * _attackDistance;
            
            if (vector.sqrMagnitude <= sqrAttackDistance
                && healthComponent.IsAlive)
            {
                attackComponent.Attack(target);
                return BehaviourResult.Running;
            }

            if (healthComponent.IsDead)
            {
                _blackboard.DelValue(BlackboardAPI.Target);
                return BehaviourResult.Success;
            }
            
            return BehaviourResult.Failure;
        }
    }
}