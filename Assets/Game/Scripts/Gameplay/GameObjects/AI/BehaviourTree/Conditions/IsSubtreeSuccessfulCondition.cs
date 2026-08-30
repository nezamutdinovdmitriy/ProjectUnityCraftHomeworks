using System;
using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace SampleGame.AI.BehaviourTree.Conditions
{
    public class IsSubtreeSuccessfulCondition : ICondition
    {
        [SerializeField]
        private List<BehaviourNode> _nodes;
        
        public bool Invoke()
        {
            if (_nodes == null || _nodes.Count == 0)
                throw new UnassignedReferenceException(
                    $"[{nameof(IsSubtreeSuccessfulCondition)}] Assign nodes in the Inspector.");
            
            Debug.Log("IsSubtreeSuccessfulCondition INVOKED!");
            
            foreach (BehaviourNode node in _nodes)
            {
                if(node == null)
                    throw new NullReferenceException(
                        $"[{nameof(IsSubtreeSuccessfulCondition)}] Element in list is null!");
                
                if (node.Result == BehaviourResult.Failure)
                    return false;
            }

            return true;
        }
    }
}