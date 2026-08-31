using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    public class CleanupAllCommandNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.TryGetValue(BlackboardAPI.CurrentCommand, out ICommandData commandData) == false
                || commandData is not StopCommandData)
                return BehaviourResult.Failure;
            
            _blackboard.GetValue(BlackboardAPI.CommandQueue).Clear();
            _blackboard.SetReferenceValue(
                BlackboardAPI.CurrentCommand, 
                new DefaultCommandData(
                    new CommandPoint(_blackboard.GetValue(BlackboardAPI.Character).transform.position)));
            return BehaviourResult.Success;
        }
    }
}