#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace Modules.AI
{
    [InitializeOnLoad]
    internal static class BlackboardKeyCache
    {
        private static readonly Dictionary<Type, List<string>> valueCache = new();
        private static readonly List<string> tagCache = new();

        static BlackboardKeyCache()
        {
            BuildCache();
        }

        // =========================
        // PUBLIC API
        // =========================

        public static IList<string> GetKeysForType(Type valueType)
        {
            if (valueType == null)
                valueType = typeof(object);

            if (valueCache.TryGetValue(valueType, out var list))
                return list;

            return Array.Empty<string>();
        }

        public static IList<string> GetAllTags()
        {
            return tagCache;
        }

        // =========================
        // BUILD
        // =========================

        private static void BuildCache()
        {
            valueCache.Clear();
            tagCache.Clear();

            var allTypes = TypeCache.GetTypesWithAttribute<BlackboardAPIAttribute>();

            foreach (var type in allTypes)
            {
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

                foreach (var field in fields)
                {
                    var fieldType = field.FieldType;

                    // -------------------------
                    // TAGS
                    // -------------------------
                    if (fieldType == typeof(BlackboardTagKey))
                    {
                        tagCache.Add(field.Name);
                        continue;
                    }

                    // -------------------------
                    // VALUE KEYS
                    // -------------------------
                    if (!fieldType.IsGenericType)
                        continue;

                    if (fieldType.GetGenericTypeDefinition() != typeof(BlackboardValueKey<>))
                        continue;

                    var valueType = fieldType.GetGenericArguments()[0];

                    AddValueKey(valueType, field.Name);
                }
            }

            // сортировка
            foreach (var kvp in valueCache)
                kvp.Value.Sort();

            tagCache.Sort();
        }

        // =========================
        // VALUE CACHE LOGIC
        // =========================

        private static void AddValueKey(Type keyType, string fieldName)
        {
            var assignableTypes = GetAllAssignableTypes(keyType);

            foreach (var type in assignableTypes)
            {
                if (!valueCache.TryGetValue(type, out var list))
                {
                    list = new List<string>();
                    valueCache[type] = list;
                }

                list.Add(fieldName);
            }
        }

        private static IEnumerable<Type> GetAllAssignableTypes(Type type)
        {
            // сам тип
            yield return type;

            // базовые классы
            var current = type.BaseType;
            while (current != null)
            {
                yield return current;
                current = current.BaseType;
            }

            // интерфейсы
            foreach (var i in type.GetInterfaces())
                yield return i;

            // универсальный fallback
            yield return typeof(object);
        }
    }
}
#endif