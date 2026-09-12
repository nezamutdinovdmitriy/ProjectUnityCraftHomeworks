using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    public class DequeueCommandNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.TryGetValue(BlackboardAPI.CommandQueue, out Queue<ICommandData> queue) == false)
                return BehaviourResult.Failure;
            
            _blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, queue.Dequeue());

            return BehaviourResult.Success;
        }
    }
}