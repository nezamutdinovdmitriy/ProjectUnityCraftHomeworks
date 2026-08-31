using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    public class ResetCurrentCommandNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.TryGetValue(BlackboardAPI.CommandQueue, out Queue<ICommandData> queueCommand)
                && queueCommand.Count > 0)
                _blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, queueCommand.Dequeue());
            else
                _blackboard.SetReferenceValue(
                    BlackboardAPI.CurrentCommand,
                    new DefaultCommandData(
                        new CommandPoint(_blackboard.GetValue(BlackboardAPI.Character).transform.position)));
            
            return BehaviourResult.Success;
        }
    }
}