using System;
using NUnit.Framework;

namespace Modules.AI
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class BlackboardTagKeyAttribute : PropertyAttribute
    {
    }
}