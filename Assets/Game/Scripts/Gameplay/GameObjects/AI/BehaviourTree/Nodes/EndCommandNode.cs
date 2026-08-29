using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace SampleGame.AI.Nodes
{
    public class EndCommandNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.TryGetValue(BlackboardAPI.CommandQueue, out Queue<ICommandData> queueCommand))
            {
                if (queueCommand.Count > 0)
                    _blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, queueCommand.Dequeue());

                return BehaviourResult.Success;
            }

            _blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, new DefaultCommandData());
            return BehaviourResult.Success;
        }
    }
}