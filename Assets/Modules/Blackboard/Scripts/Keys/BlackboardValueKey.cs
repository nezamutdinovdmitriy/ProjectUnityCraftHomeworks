using System;

namespace Modules.AI
{
    public readonly struct BlackboardValueKey<T> : IEquatable<BlackboardValueKey<T>>
    {
        /// <summary>
        /// Internal identifier of the key.
        /// </summary>
        internal readonly int Id;

        /// <summary>
        /// Creates a value key from a string name.
        /// </summary>
        /// <param name="name">The name of the value.</param>
        public BlackboardValueKey(string name)
        {
            Id = BlackboardKeys.NameToId(name);
        }

        /// <summary>
        /// Creates a value key from an existing identifier.
        /// </summary>
        /// <param name="id">The numeric identifier.</param>
        public BlackboardValueKey(int id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Returns the string representation of the key.
        /// </summary>
        /// <returns>The name associated with this key.</returns>
        public override string ToString() => BlackboardKeys.IdToName(Id);

        /// <summary>
        /// Determines whether the specified key is equal to the current key.
        /// </summary>
        public bool Equals(BlackboardValueKey<T> other) => Id == other.Id;

        public bool Equals(int id) => Id == id;

        public bool Equals(string name) => Id == BlackboardKeys.NameToId(name);

        /// <summary>
        /// Determines whether the specified object is equal to the current key.
        /// </summary>
        public override bool Equals(object obj) => obj is BlackboardValueKey<T> other && Equals(other);

        /// <summary>
        /// Returns the hash code for this key.
        /// </summary>
        public override int GetHashCode() => Id;

        public static bool operator ==(BlackboardValueKey<T> left, BlackboardValueKey<T> right) => 
            left.Id == right.Id;

        public static bool operator !=(BlackboardValueKey<T> left, BlackboardValueKey<T> right) => 
            left.Id != right.Id;

        public static bool operator ==(BlackboardValueKey<T> left, int right) => 
            left.Id == right;

        public static bool operator !=(BlackboardValueKey<T> left, int right) => 
            left.Id != right;

        public static bool operator ==(BlackboardValueKey<T> left, string right) =>
            left.Id == BlackboardKeys.NameToId(right);

        public static bool operator !=(BlackboardValueKey<T> left, string right) =>
            left.Id != BlackboardKeys.NameToId(right);
    }
}