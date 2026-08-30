using System;
using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    public class FindNearestEnemyNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;

        [SerializeField]
        private float _detectRadius;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.TryGetValue(BlackboardAPI.ColliderBuffer, out Collider[] buffer) == false
                || _blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) == false
                || _blackboard.TryGetValue(BlackboardAPI.Target, out GameObject target) == false
                || character.TryGetComponent(out TeamComponent selfTeamComponent) == false)
                return BehaviourResult.Failure;

            int size = Physics.OverlapSphereNonAlloc(character.transform.position, _detectRadius, buffer);

            float minSqrDistance = float.MaxValue;
            
            GameObject nearestTarget = null;

            for (int i = 0; i < size; i++)
            {
                Collider collider = buffer[i];

                if (collider.TryGetComponent(out TeamComponent teamComponent)
                    && selfTeamComponent.Team != teamComponent.Team)
                {
                    float sqrDistance = (collider.transform.position - character.transform.position).sqrMagnitude;
                    
                    if (sqrDistance < minSqrDistance)
                    {
                        minSqrDistance = sqrDistance;
                        nearestTarget = collider.gameObject;
                    }
                }
            }

            if (nearestTarget != null)
            {
                _blackboard.SetReferenceValue(BlackboardAPI.Target, nearestTarget);
                return BehaviourResult.Success;
            }
            
            _blackboard.SetReferenceValue(BlackboardAPI.Target, null);
            return BehaviourResult.Failure;
        }
    }
}