using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static Modules.AI.BlackboardKeys;

namespace Modules.AI
{
    public static partial class Extensions
    {
        
        
        #region AddTag

        /// <summary>
        /// Adds a tag to the blackboard.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AddTag(this Blackboard blackboard, string tag) => blackboard.AddTag(NameToId(tag));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AddTag(this Blackboard blackboard, BlackboardTagKey tag) => blackboard.AddTag(tag.Id);

        /// <summary>
        /// Adds a tag to the blackboard and returns its numeric ID.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AddTag(this Blackboard blackboard, string tag, out int id)
        {
            id = NameToId(tag);
            return blackboard.AddTag(id);
        }

        #endregion

        #region AddTags

        /// <summary>
        /// Adds multiple tags to the blackboard.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddTags(this Blackboard blackboard, IEnumerable<int> tags)
        {
            if (tags == null)
                return;

            foreach (int tag in tags)
                blackboard.AddTag(tag);
        }

        /// <summary>
        /// Adds multiple tags by string identifiers.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddTags(this Blackboard blackboard, IEnumerable<string> tags)
        {
            if (tags == null)
                return;

            foreach (string tag in tags)
                blackboard.AddTag(tag);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddTags(this Blackboard blackboard, IEnumerable<BlackboardTagKey> tags)
        {
            if (tags == null)
                return;

            foreach (BlackboardTagKey tag in tags)
                blackboard.AddTag(tag.Id);
        }

        #endregion

        #region DelTag

        /// <summary>
        /// Removes a tag from the blackboard.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool DelTag(this Blackboard blackboard, string tag) => blackboard.DelTag(NameToId(tag));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool DelTag(this Blackboard blackboard, BlackboardTagKey tag) => blackboard.DelTag(tag.Id);
        #endregion

        #region HasTag

        /// <summary>
        /// Checks if the blackboard has the specified tag.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasTag(this Blackboard blackboard, string tag) => blackboard.HasTag(NameToId(tag));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasTag(this Blackboard blackboard, BlackboardTagKey tag) => blackboard.HasTag(tag.Id);
        
        #endregion

        #region HasAllTags

        /// <summary>
        /// Checks if the blackboard contains all of the specified tags.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAllTags(this Blackboard blackboard, params int[] tags)
        {
            for (int i = 0, count = tags.Length; i < count; i++)
                if (!blackboard.HasTag(tags[i]))
                    return false;

            return true;
        }

        /// <summary>
        /// Checks if the blackboard has all the specified tags.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAllTags(this Blackboard blackboard, params string[] tags)
        {
            for (int i = 0, count = tags.Length; i < count; i++)
                if (!blackboard.HasTag(tags[i]))
                    return false;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAllTags(this Blackboard blackboard, params BlackboardTagKey[] tags)
        {
            for (int i = 0, count = tags.Length; i < count; i++)
                if (!blackboard.HasTag(tags[i].Id))
                    return false;

            return true;
        }
        
        #endregion

        #region HasAnyTag

        /// <summary>
        /// Checks if the blackboard has any of the specified tags.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAnyTag(this Blackboard blackboard, params string[] tags)
        {
            for (int i = 0, count = tags.Length; i < count; i++)
                if (blackboard.HasTag(tags[i]))
                    return true;

            return false;
        }

        /// <summary>
        /// Checks if the blackboard contains any of the specified tags.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAnyTag(this Blackboard blackboard, params int[] tags)
        {
            for (int i = 0, count = tags.Length; i < count; i++)
                if (blackboard.HasTag(tags[i]))
                    return true;

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAnyTag(this Blackboard blackboard, params BlackboardTagKey[] tags)
        {
            for (int i = 0, count = tags.Length; i < count; i++)
                if (blackboard.HasTag(tags[i].Id))
                    return true;

            return false;
        }

        #endregion

        #region SetTag
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SetTag(this Blackboard blackboard, BlackboardTagKey tag, bool add) => 
            add ? blackboard.AddTag(tag) : blackboard.DelTag(tag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SetTag(this Blackboard blackboard, string tag, bool add) => 
            add ? blackboard.AddTag(tag) : blackboard.DelTag(tag);

        #endregion
    }
}