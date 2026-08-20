using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.AI
{
    [Serializable, InlineProperty]
    public sealed class IntegerBlackboardInstaller : IBlackboardInstaller
    {
        [SerializeField]
        [BlackboardValueKey(typeof(int))]
        private string key;

        [SerializeField]
        private int value;

        public void Install(Blackboard blackboard)
        {
            blackboard.AddPrimitiveValue(key, this.value);
        }
    }
}