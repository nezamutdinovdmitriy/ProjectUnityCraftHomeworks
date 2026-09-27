using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    public class AssignNearestEnemyAsTarget : BehaviourNode
    {
        [SerializeField] private Blackboard _blackboard;

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.TryGetValue(BlackboardAPI.NearestEnemy, out GameObject enemy)
                && enemy != null)
            {
                _blackboard.SetReferenceValue(BlackboardAPI.Target, enemy);
                return BehaviourResult.Success;
            }
            
            _blackboard.DelValue(BlackboardAPI.Target);
            return BehaviourResult.Failure;
        }
    }
}