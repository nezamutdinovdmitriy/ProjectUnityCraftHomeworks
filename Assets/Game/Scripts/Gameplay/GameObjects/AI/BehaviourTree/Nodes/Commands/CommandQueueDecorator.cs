using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    public class CommandQueueDecorator : BehaviourNode
    {
        [SerializeField] private Blackboard _blackboard;
        [SerializeField] private BehaviourNode _child;

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            TrySetQueuedCommand();

            BehaviourResult result = _child.Run(deltaTime);

            if (result == BehaviourResult.Running)
                return BehaviourResult.Running;

            _blackboard.DelValue(BlackboardAPI.CurrentCommand);

            if (TrySetQueuedCommand())
                return BehaviourResult.Running;

            return result;
        }

        protected override void OnAbort()
        {
            if(_child.IsRunning)
                _child.Abort();
        }

        private bool TrySetQueuedCommand()
        {
            if (_blackboard.HasValue(BlackboardAPI.CurrentCommand)
                || _blackboard.TryGetValue(BlackboardAPI.CommandQueue, out Queue<ICommandData> queue) == false
                || queue.Count == 0)
                return false;
            
            _blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, queue.Dequeue());

            return true;
        }
    }
}