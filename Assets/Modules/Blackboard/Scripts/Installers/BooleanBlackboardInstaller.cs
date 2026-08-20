using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.AI
{
    [Serializable, InlineProperty]
    public sealed class BooleanBlackboardInstaller : IBlackboardInstaller
    {
        [SerializeField]
        [BlackboardValueKey(typeof(bool))]
        private string key;

        [SerializeField]
        private bool value;

        public void Install(Blackboard blackboard)
        {
            blackboard.AddPrimitiveValue(key, this.value);
        }
    }
}