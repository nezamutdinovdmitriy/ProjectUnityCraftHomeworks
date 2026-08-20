using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.AI
{
    [Serializable, InlineProperty]
    public sealed class FloatBlackboardInstaller : IBlackboardInstaller
    {
        [SerializeField]
        [BlackboardValueKey(typeof(float))]
        private string key;

        [SerializeField]
        private float value;

        public void Install(Blackboard blackboard)
        {
            blackboard.AddPrimitiveValue(key, this.value);
        }
    }
}