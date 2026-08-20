using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Modules.AI
{
    [Serializable]
    public sealed class OrCondition : ICondition
    {
        [SerializeReference, HideLabel]
        private ICondition[] conditions = Array.Empty<ICondition>();

        public OrCondition()
        {
        }

        public OrCondition(ICondition[] conditions)
        {
            this.conditions = conditions;
        }

        public bool Invoke()
        {
            if (this.conditions == null)
                return false;
            
            for (int i = 0, count = this.conditions.Length; i < count; i++)
                if (this.conditions[i].Invoke())
                    return true;

            return false;
        }
    }
}