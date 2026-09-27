using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace SampleGame.AI.BehaviourTree.Conditions
{
    public class HasCommandsInQueue : ICondition
    {
        [SerializeField] private Blackboard _blackboard;
        
        public bool Invoke()
        {
            return _blackboard.TryGetValue(BlackboardAPI.CommandQueue, out Queue<ICommandData> queue)
                   && queue.Count > 0;
        }
    }
}