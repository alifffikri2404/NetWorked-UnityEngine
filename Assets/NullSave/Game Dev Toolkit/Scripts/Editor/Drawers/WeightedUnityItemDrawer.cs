using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    [CustomPropertyDrawer(typeof(WeightedUnityItem))]
    public class WeightedUnityItemDrawer : PropertyDrawer
    {

        #region Public Methods

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int lines = 4;

            if(property.FindPropertyRelative("UseRandomCount").boolValue)
            {
                lines += 1;
            }

            if(property.FindPropertyRelative("randomRotation").boolValue)
            {
                lines += 2;
            }

            return (EditorGUIUtility.singleLineHeight + 2) * lines;
        }

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            label.text = string.Empty;
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            //// Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var rect = position;
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("Object"), new GUIContent("Object", null, string.Empty));
            rect.y += rect.height + 2;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("UseRandomCount"), new GUIContent("Random Count", null, string.Empty));
            rect.y += rect.height + 2;

            if (property.FindPropertyRelative("UseRandomCount").boolValue)
            {
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("MinCount"), new GUIContent("Minimum", null, string.Empty));
                rect.y += rect.height + 2;
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("MaxCount"), new GUIContent("Maximum", null, string.Empty));
                rect.y += rect.height + 2;
            }
            else
            {
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("Count"), new GUIContent("Count", null, string.Empty));
            }

            EditorGUI.PropertyField(rect, property.FindPropertyRelative("randomRotation"), new GUIContent("Random Rotation", null, string.Empty));
            rect.y += rect.height + 2;
            if (property.FindPropertyRelative("randomRotation").boolValue)
            {
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("minRotation"), new GUIContent("Minimum", null, string.Empty));
                rect.y += rect.height + 2;
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("maxRotation"), new GUIContent("Maximum", null, string.Empty));
                rect.y += rect.height + 2;
            }


            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        #endregion

    }
}