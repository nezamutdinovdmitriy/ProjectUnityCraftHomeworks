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
            if (_blackboard.TryGetValue(BlackboardAPI.PatrolPointIndex, out int index)
                && _blackboard.TryGetValue(BlackboardAPI.CurrentCommand, out ICommandData commandData)
                && commandData is PatrolCommandData patrolCommand)
            {
                int nextIndex = (index + 1) % patrolCommand.Points.Count;

                _blackboard.SetPrimitiveValue(BlackboardAPI.PatrolPointIndex, nextIndex);

                return BehaviourResult.Success;
            }

            return BehaviourResult.Failure;
        }
    }
}