using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Attributes;
using Core.Utils;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(ConstantDropdown))]
    public class ConstantDropdownDrawer : PropertyDrawer
    {
        private string[] _cachedConstants;

        private string[] GetConstants(Type type)
        {
            if (_cachedConstants != null) return _cachedConstants;

            var constants = new List<string>();

            if (type == null)
            {
                EditorLogger.LogError("Target type is null.");
                return _cachedConstants = constants.ToArray();
            }

            var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            foreach (var fi in fieldInfos)
            {
                if (fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                    constants.Add((string)fi.GetRawConstantValue());
            }

            return _cachedConstants = constants.ToArray();
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            if (property.propertyType == SerializedPropertyType.String)
            {
                var attr = (ConstantDropdown)attribute;
                var constants = GetConstants(attr.TargetType);

                if (constants == null || constants.Length == 0)
                {
                    EditorGUI.LabelField(rect, label.text, "No constants found in target type");
                    EditorGUI.EndProperty();
                    return;
                }

                var propertyString = property.stringValue;
                var index = -1;

                for (var i = 0; i < constants.Length; i++)
                {
                    if (constants[i].Equals(propertyString, StringComparison.Ordinal))
                    {
                        index = i;
                        break;
                    }
                }

                if (index == -1)
                {
                    var helpRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
                    var popupRect = new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight + 2, rect.width, EditorGUIUtility.singleLineHeight);

                    EditorGUI.HelpBox(helpRect, $"Missing: '{propertyString}'", MessageType.Warning);

                    var optionsWithMissing = new string[constants.Length + 1];
                    optionsWithMissing[0] = $"(Missing) {propertyString}";
                    Array.Copy(constants, 0, optionsWithMissing, 1, constants.Length);

                    var newIndex = EditorGUI.Popup(popupRect, label.text, 0, optionsWithMissing);

                    if (newIndex > 0)
                        property.stringValue = constants[newIndex - 1];

                    EditorGUI.EndProperty();
                    return;
                }

                var selectedIndex = EditorGUI.Popup(rect, label.text, index, constants);
                var newValue = selectedIndex >= 0 ? constants[selectedIndex] : string.Empty;

                if (!property.stringValue.Equals(newValue, StringComparison.Ordinal))
                    property.stringValue = newValue;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var constants = GetConstants(((ConstantDropdown)attribute).TargetType);
            var isMissing = constants.All(c => !c.Equals(property.stringValue, StringComparison.Ordinal));

            return isMissing
                ? EditorGUIUtility.singleLineHeight * 2 + 2
                : EditorGUI.GetPropertyHeight(property, includeChildren: true);
        }
    }
}