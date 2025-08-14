using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    [CustomPropertyDrawer(typeof(AnimatorModifier))]
    public class AnimatorModifierDrawer : PropertyDrawer
    {

        #region Public Methods

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 3 + 6;
        }

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            //// Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var rect = position;
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("keyName"), new GUIContent("Key Name", null, string.Empty));
            rect.y += rect.height + 2;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("paramType"), new GUIContent("Param Type", null, string.Empty));
            rect.y += rect.height + 2;

            switch ((AnimatorParamType)property.FindPropertyRelative("paramType").intValue)
            {
                case AnimatorParamType.Bool:
                    EditorGUI.PropertyField(rect, property.FindPropertyRelative("boolVal"), new GUIContent("Value", null, string.Empty));
                    break;
                case AnimatorParamType.Int:
                    EditorGUI.PropertyField(rect, property.FindPropertyRelative("intVal"), new GUIContent("Value", null, string.Empty));
                    break;
                case AnimatorParamType.Float:
                    EditorGUI.PropertyField(rect, property.FindPropertyRelative("floatVal"), new GUIContent("Value", null, string.Empty));
                    break;
                case AnimatorParamType.Trigger:
                    EditorGUI.PropertyField(rect, property.FindPropertyRelative("triggerVal"), new GUIContent("Value", null, string.Empty));
                    break;
            }

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        #endregion

    }
}