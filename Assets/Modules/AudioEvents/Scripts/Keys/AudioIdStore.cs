
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Modules.AudioEvents
{
    /// <summary>
    /// Provides a unified interface for converting audio event names
    /// into integer identifiers, with caching and reverse lookup.
    /// </summary>
    public static class AudioIdStore
    {
        private static readonly Dictionary<string, int> s_nameToId = new();
        private static readonly Dictionary<int, string> s_idToName = new();

        private static IAudioIdGenerator s_generator = new Fnv1AAudioIdGenerator();
        
        /// <summary>
        /// Sets the id generator used for generating audio event IDs.
        /// Clears all existing mappings.
        /// </summary>
        public static void SetGenerator(IAudioIdGenerator generator)
        {
            if (generator == null)
                throw new ArgumentNullException(nameof(generator));

            s_generator.Reset();
            s_generator = generator;

            s_nameToId.Clear();
            s_idToName.Clear();
        }

        /// <summary>
        /// Converts an audio event name into an integer ID.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int NameToId(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (s_nameToId.TryGetValue(name, out int id))
                return id;

            id = s_generator.GenerateId(name);

            if (s_idToName.TryGetValue(id, out string existingName) && existingName != name)
            {
                throw new InvalidOperationException(
                    $"Audio ID collision: '{name}' and '{existingName}' " +
                    $"have the same ID ({id}).");
            }

            s_nameToId.Add(name, id);
            s_idToName.Add(id, name);

            return id;
        }

        /// <summary>
        /// Retrieves the original audio event name for a given ID.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string IdToName(int id)
        {
            return s_idToName.TryGetValue(id, out string name)
                ? name
                : $"#Unknown:{id}";
        }

#if UNITY_EDITOR
        [InitializeOnEnterPlayMode]
        private static void ResetOnEnterPlayMode(EnterPlayModeOptions options)
        {
            Reset();
        }
#endif

        /// <summary>
        /// Clears all cached mappings and resets the current algorithm.
        /// </summary>
        public static void Reset()
        {
            s_nameToId.Clear();
            s_idToName.Clear();
            s_generator.Reset();
        }
    }
}
