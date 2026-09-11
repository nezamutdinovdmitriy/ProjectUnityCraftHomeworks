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
            Queue<ICommandData> queue = _blackboard.GetValue(BlackboardAPI.CommandQueue);
            
            _blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, queue.Dequeue());

            return BehaviourResult.Success;
            
            // if (_blackboard.TryGetValue(BlackboardAPI.CommandQueue, out Queue<ICommandData> queue))
            // {
            //     ICommandData nextCommand = queue.Dequeue();
            //     _blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, nextCommand);
            //
            //     return BehaviourResult.Success;
            // }
            //
            // return BehaviourResult.Failure;
        }
    }
}