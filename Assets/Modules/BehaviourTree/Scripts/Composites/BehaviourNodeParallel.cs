using System.Collections.Generic;
using UnityEngine;

namespace Modules.AI
{
    [AddComponentMenu("AI/BehaviourTree/BehaviourNode «Parallel»")]
    public sealed class BehaviourNodeParallel : BehaviourNode, IBehaviourNodeComposite
    {
        public IEnumerable<BehaviourNode> Nodes => _nodes;

        [Space]
        [SerializeField]
        private BehaviourNode[] _nodes;

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            foreach (BehaviourNode node in _nodes)
                node.Run(deltaTime);

            return BehaviourResult.Running;
        }

        protected override void OnAbort()
        {
            foreach (BehaviourNode node in _nodes)
                node.Abort();
        }
    }
}