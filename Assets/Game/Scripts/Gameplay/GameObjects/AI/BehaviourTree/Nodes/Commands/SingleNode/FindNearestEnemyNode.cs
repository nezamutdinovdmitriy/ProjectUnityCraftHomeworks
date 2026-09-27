using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    public class FindNearestEnemyNode : BehaviourNode
    {
        [SerializeField] private Blackboard _blackboard;

        [SerializeField] private float _detectRadius;

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.TryGetValue(BlackboardAPI.ColliderBuffer, out Collider[] buffer) == false
                || _blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) == false
                || character.TryGetComponent(out TeamComponent selfTeamComponent) == false)
                return BehaviourResult.Failure;


            if (TryGetNearestTarget(buffer, character, selfTeamComponent, out GameObject nearestTarget))
            {
                _blackboard.SetReferenceValue(BlackboardAPI.Target, nearestTarget);
                return BehaviourResult.Success;
            }

            _blackboard.DelValue(BlackboardAPI.Target);
            return BehaviourResult.Failure;
        }

        private bool TryGetNearestTarget(Collider[] buffer, GameObject self, TeamComponent selfTeamComponent,
            out GameObject nearestTarget)
        {
            Vector3 selfPosition = self.transform.position;

            int size = Physics.OverlapSphereNonAlloc(selfPosition, _detectRadius, buffer);

            float minSqrDistance = float.MaxValue;
            nearestTarget = null;

            for (int i = 0; i < size; i++)
            {
                Collider collider = buffer[i];

                if (collider.TryGetComponent(out TeamComponent teamComponent) == false
                    || selfTeamComponent.Team == teamComponent.Team)
                    continue;

                float sqrDistance = (collider.transform.position - selfPosition).sqrMagnitude;

                if (sqrDistance >= minSqrDistance)
                    continue;

                minSqrDistance = sqrDistance;
                nearestTarget = collider.gameObject;
            }

            return nearestTarget != null;
        }
    }
}