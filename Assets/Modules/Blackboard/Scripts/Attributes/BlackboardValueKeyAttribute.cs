using System;
using UnityEngine;

namespace Modules.AI
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class BlackboardValueKeyAttribute : PropertyAttribute
    {
        public readonly Type valueType;

        public BlackboardValueKeyAttribute(Type valueType)
        {
            this.valueType = valueType;
        }
    }
}