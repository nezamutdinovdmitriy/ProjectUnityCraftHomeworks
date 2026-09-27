using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    public class ClearCommandQueueNode : BehaviourNode
    {
        [SerializeField] private Blackboard _blackboard;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.TryGetValue(BlackboardAPI.CommandQueue, out Queue<ICommandData> queue))
                queue.Clear();
            
            return BehaviourResult.Success;
        }
    }
}