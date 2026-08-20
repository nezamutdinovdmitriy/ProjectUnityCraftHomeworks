using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static Modules.AI.BlackboardKeys;

namespace Modules.AI
{
    public static partial class Extensions
    {
        #region AddValue

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddReferenceValue<T>(this Blackboard entity, BlackboardValueKey<T> key, T value)
            where T : class =>
            entity.AddReferenceValue(key.Id, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddPrimitiveValue<T>(this Blackboard entity, BlackboardValueKey<T> key, T value)
            where T : struct =>
            entity.AddPrimitiveValue(key.Id, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddReferenceValue<T>(this Blackboard entity, string key, T value) where T : class =>
            entity.AddReferenceValue(NameToId(key), value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddPrimitiveValue<T>(this Blackboard entity, string key, T value) where T : struct =>
            entity.AddPrimitiveValue(NameToId(key), value);

        #endregion

        #region AddValues

        /// <summary>
        /// Adds multiple values to the entity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddValues(this Blackboard entity, IEnumerable<KeyValuePair<int, object>> values)
        {
            if (values != null)
                foreach ((int key, object value) in values)
                    entity.AddReferenceValue(key, value);
        }

        #endregion

        #region DelValue

        /// <summary>
        /// Removes a value from the entity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool DelValue(this Blackboard entity, string key) =>
            entity.DelValue(NameToId(key));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool DelValue<T>(this Blackboard entity, BlackboardValueKey<T> key) =>
            entity.DelValue(key.Id);

        #endregion

        #region GetValue

        /// <summary>
        /// Retrieves a value of type T associated with the given key.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetValue<T>(this Blackboard entity, string key) =>
            entity.GetValue<T>(NameToId(key));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetValue(this Blackboard entity, string key) =>
            entity.GetValue(NameToId(key));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetValue<T>(this Blackboard entity, BlackboardValueKey<T> key) =>
            entity.GetValue<T>(key.Id);

        #endregion

        #region TryGetValue

        /// <summary>
        /// Tries to retrieve a value of type T associated with the given key.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetValue<T>(this Blackboard entity, string key, out T value) =>
            entity.TryGetValue(NameToId(key), out value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetValue(this Blackboard entity, string key, out object value) =>
            entity.TryGetValue(NameToId(key), out value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetValue<T>(this Blackboard entity, BlackboardValueKey<T> key, out T value) =>
            entity.TryGetValue(key.Id, out value);

        #endregion

        #region GetValueUnsafe

        /// <summary>
        /// Retrieves a value of type T associated with the given key.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetValueUnsafe<T>(this Blackboard entity, string key) where T : class =>
            ref entity.GetValueUnsafe<T>(NameToId(key));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetValueUnsafe<T>(this Blackboard entity, BlackboardValueKey<T> key) where T : class =>
            ref entity.GetValueUnsafe<T>(key.Id);

        #endregion


        #region SetValue

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetReferenceValue<T>(this Blackboard entity, BlackboardValueKey<T> key, T value)
            where T : class =>
            entity.SetReferenceValue(key.Id, value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPrimitiveValue<T>(this Blackboard entity, BlackboardValueKey<T> key, T value)
            where T : struct =>
            entity.SetPrimitiveValue(key.Id, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetReferenceValue<T>(this Blackboard entity, string key, T value)
            where T : class =>
            entity.SetReferenceValue(NameToId(key), value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPrimitiveValue<T>(this Blackboard entity, string key, T value)
            where T : struct =>
            entity.SetPrimitiveValue(NameToId(key), value);

        
        #endregion

        #region HasValue

        /// <summary>
        /// Checks if the entity has a value with the given key.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasValue(this Blackboard entity, string key) => entity.HasValue(NameToId(key));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasValue<T>(this Blackboard entity, BlackboardValueKey<T> key) =>
            entity.HasValue(key.Id);

        #endregion

        /// <summary>
        /// Disposes all disposable values stored in the entity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DisposeValues(this Blackboard entity)
        {
            KeyValuePair<int, object>[] pairs = entity.GetValues();
            for (int i = 0, count = pairs.Length; i < count; i++)
                if (pairs[i].Value is IDisposable disposable)
                    disposable.Dispose();
        }
    }
}