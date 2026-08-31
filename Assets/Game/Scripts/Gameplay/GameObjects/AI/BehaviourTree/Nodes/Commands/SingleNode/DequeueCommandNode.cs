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
            if (_blackboard.TryGetValue(BlackboardAPI.CommandQueue, out Queue<ICommandData> queue)
                && queue.Count > 0)
            {
                ICommandData nextCommand = queue.Dequeue();
                _blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, nextCommand);

                return BehaviourResult.Success;
            }

            return BehaviourResult.Failure;
        }
    }
}