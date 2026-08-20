using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.AI
{
    [Serializable, InlineProperty]
    public sealed class GameObjectBlackboardInstaller : IBlackboardInstaller
    {
        [SerializeField]
        [BlackboardValueKey(typeof(GameObject))]
        private string key;

        [SerializeField]
        private GameObject value;

        public void Install(Blackboard blackboard)
        {
            blackboard.AddReferenceValue(key, this.value);
        }
    }
}