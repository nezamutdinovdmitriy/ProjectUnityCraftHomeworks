using System;

namespace Modules.AI
{
    public readonly struct BlackboardTagKey : IEquatable<BlackboardTagKey>
    {
        /// <summary>
        /// Unique identifier of the tag.
        /// </summary>
        internal readonly int Id;

        /// <summary>
        /// Creates a tag key from a string name.
        /// </summary>
        /// <param name="name">Tag name.</param>
        public BlackboardTagKey(string name) => Id = BlackboardKeys.NameToId(name);

        /// <summary>
        /// Creates a tag key from an existing identifier.
        /// </summary>
        /// <param name="id">Numeric tag identifier.</param>
        public BlackboardTagKey(int id) => this.Id = id;

        /// <summary>
        /// Returns the string representation of the tag.
        /// </summary>
        /// <returns>Tag name.</returns>
        public override string ToString() => BlackboardKeys.IdToName(Id);
        
        public bool Equals(BlackboardTagKey other) => Id == other.Id;

        public override bool Equals(object obj) => obj is BlackboardTagKey other && Equals(other);

        public override int GetHashCode() => Id;
        
        public static bool operator ==(BlackboardTagKey left, BlackboardTagKey right) => left.Id == right.Id;

        public static bool operator !=(BlackboardTagKey left, BlackboardTagKey right) => left.Id != right.Id;

        public static bool operator ==(BlackboardTagKey left, int right) => left.Id == right;

        public static bool operator !=(BlackboardTagKey left, int right) => left.Id != right;

        public static bool operator ==(int right, BlackboardTagKey left) => left.Id == right;

        public static bool operator !=(int right, BlackboardTagKey left) => left.Id != right;
        
        public static bool operator ==(BlackboardTagKey left, string right) => left.Id == BlackboardKeys.NameToId(right);

        public static bool operator !=(BlackboardTagKey left, string right) => left.Id != BlackboardKeys.NameToId(right);
        
        public static bool operator ==(string right, BlackboardTagKey left) => left.Id == BlackboardKeys.NameToId(right);

        public static bool operator !=(string right, BlackboardTagKey left) => left.Id != BlackboardKeys.NameToId(right);

    }
}