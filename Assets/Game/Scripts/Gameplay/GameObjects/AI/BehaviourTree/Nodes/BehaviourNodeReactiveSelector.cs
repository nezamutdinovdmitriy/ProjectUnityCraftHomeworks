using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace SampleGame.AI.BehaviourTree.Nodes
{
    public class BehaviourNodeReactiveSelector : BehaviourNode, IBehaviourNodeComposite
    {
        [SerializeField] private BehaviourNode[] _nodes;

        private BehaviourNode _runningNode;

        public IEnumerable<BehaviourNode> Nodes => _nodes;

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            foreach (BehaviourNode node in _nodes)
            {
                BehaviourResult result = node.Run(deltaTime);

                if (result == BehaviourResult.Failure)
                    continue;

                if (_runningNode != null && _runningNode != node && _runningNode.IsRunning)
                    _runningNode.Abort();

                _runningNode = result == BehaviourResult.Running ? node : null;

                return result;
            }

            if (_runningNode != null && _runningNode.IsRunning)
                _runningNode.Abort();

            _runningNode = null;

            return BehaviourResult.Failure;
        }
        
        protected override void OnAbort()
        {
            if (_runningNode != null && _runningNode.IsRunning)
                _runningNode.Abort();

            _runningNode = null;
        }
    }
}