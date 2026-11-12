using System;

namespace Modules.AudioEvents
{
    /// <summary>
    /// Implements deterministic FNV-1a 32-bit hashing for audio event names.
    /// </summary>
    public sealed class Fnv1AAudioIdGenerator : IAudioIdGenerator
    {
        private const uint OFFSET_BASIS = 2166136261;
        private const uint PRIME = 16777619;

        public int GenerateId(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            unchecked
            {
                uint hash = OFFSET_BASIS;

                foreach (char c in name)
                {
                    hash ^= (byte) c;
                    hash *= PRIME;

                    hash ^= (byte) (c >> 8);
                    hash *= PRIME;
                }

                return (int) hash;
            }
        }

        public void Reset()
        {
            // Stateless algorithm.
        }
    }
}