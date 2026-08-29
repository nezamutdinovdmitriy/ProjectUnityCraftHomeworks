using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    public class StopCommandNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.TryGetValue(BlackboardAPI.CurrentCommand, out ICommandData commandData) == false
                || commandData is not StopCommandData)
                return BehaviourResult.Failure;
            
            // TODO: Добавить отчистку очереди.
            
            _blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, new DefaultCommandData());
            return BehaviourResult.Success;
        }
    }
}