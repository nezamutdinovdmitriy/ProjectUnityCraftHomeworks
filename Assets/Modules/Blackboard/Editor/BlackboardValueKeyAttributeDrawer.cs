using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Modules.AI
{
    internal sealed class BlackboardValueKeyAttributeDrawer : OdinAttributeDrawer<BlackboardValueKeyAttribute, string>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var expectedType = this.Attribute.valueType;
            IList<string> keys = BlackboardKeyCache.GetKeysForType(expectedType);

            if (keys == null || keys.Count == 0)
            {
                CallNextDrawer(label);
                return;
            }

            var property = this.ValueEntry;

            int index = keys.IndexOf(property.SmartValue);
            if (index < 0) index = 0;

            int newIndex = EditorGUILayout.Popup(label, index, ToArray(keys));

            if (newIndex >= 0 && newIndex < keys.Count)
            {
                property.SmartValue = keys[newIndex];
            }
        }

        private static string[] ToArray(IList<string> list)
        {
            var array = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
                array[i] = list[i];
            return array;
        }
    }
}