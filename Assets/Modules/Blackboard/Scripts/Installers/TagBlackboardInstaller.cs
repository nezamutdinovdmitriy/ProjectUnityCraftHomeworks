using System;
using UnityEngine;

namespace Modules.AI
{
    [Serializable]
    public sealed class TagBlackboardInstaller : IBlackboardInstaller
    {
        [SerializeField]
        [BlackboardTagKey]
        public string tag;
        
        public void Install(Blackboard blackboard)
        {
            blackboard.AddTag(this.tag);
        }
    }
}