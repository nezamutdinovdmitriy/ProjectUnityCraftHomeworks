using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.AI
{
    [AddComponentMenu("AI/BehaviourTree/BehaviourNode «Sequence»")]
    public sealed class BehaviourNodeSequence : BehaviourNode, IBehaviourNodeComposite
    {
        public IEnumerable<BehaviourNode> Nodes => _nodes;
        
        [Space]
        [SerializeField]
        private BehaviourNode[] _nodes;
        
        [HideInEditorMode]
        [ShowInInspector, ReadOnly]
        private int _nodeIndex;

        protected override void OnStart()
        {
            _nodeIndex = 0;
        }

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            while (_nodeIndex < _nodes.Length)
            {
                BehaviourNode currentNode = _nodes[_nodeIndex];
                BehaviourResult result = currentNode.Run(deltaTime);

                switch (result)
                {
                    case BehaviourResult.Success:
                    {
                        _nodeIndex++;
                        continue;
                    }

                    case BehaviourResult.Failure:
                    case BehaviourResult.Running:
                    case BehaviourResult.Aborted:
                        return result;

                    default:
                        return BehaviourResult.Failure;
                }
            }

            return BehaviourResult.Success;
        }

        protected override void OnAbort()
        {
            if (_nodeIndex >= 0 && _nodeIndex < _nodes.Length)
            {
                BehaviourNode currentNode = _nodes[_nodeIndex];
                if (currentNode.IsRunning)
                    currentNode.Abort();
            }
        }
    }
}