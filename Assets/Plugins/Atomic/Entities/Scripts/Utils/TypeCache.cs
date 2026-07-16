using System.Runtime.CompilerServices;

namespace Atomic.Entities
{
    internal static class TypeCache<T>
    {
        public static readonly bool IsUnmanagedType = !RuntimeHelpers.IsReferenceOrContainsReferences<T>();
        public static readonly bool IsValueType = typeof(T).IsValueType;
    }
}