using System;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.AI
{
    [Serializable]
    public sealed class AndCondition : ICondition
    {
        [SerializeReference, HideLabel]
        private ICondition[] conditions = Array.Empty<ICondition>();

        public AndCondition()
        {
        }

        public AndCondition(ICondition[] conditions)
        {
            this.conditions = conditions;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Invoke()
        {
            if (this.conditions == null)
                return true;
            
            for (int i = 0, count = this.conditions.Length; i < count; i++)
                if (!this.conditions[i].Invoke())
                    return false;

            return true;
        }
    }
}