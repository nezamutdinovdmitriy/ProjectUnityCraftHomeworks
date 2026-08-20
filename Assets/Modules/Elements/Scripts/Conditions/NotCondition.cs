using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.AI
{
    [Serializable]
    public sealed class NotCondition : ICondition
    {
        [SerializeReference, HideLabel]
        private ICondition condition;
        
        public bool Invoke()
        {
            return this.condition != null && !this.condition.Invoke();
        }
    }
}