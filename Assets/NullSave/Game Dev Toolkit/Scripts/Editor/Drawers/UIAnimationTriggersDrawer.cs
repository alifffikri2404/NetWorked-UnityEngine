using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    [CustomPropertyDrawer(typeof(UIAnimationTriggers))]
    public class UIAnimationTriggersDrawer : PropertyDrawer
    {

        #region Public Methods

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (EditorGUIUtility.singleLineHeight + 2) * 5;
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
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("normalTrigger"), new GUIContent("Normal", null, string.Empty));
            rect.y += rect.height + 2;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("selectedTrigger"), new GUIContent("Selected", null, string.Empty));
            rect.y += rect.height + 2;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("highlightedTrigger"), new GUIContent("Highlighted", null, string.Empty));
            rect.y += rect.height + 2;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("pressedTrigger"), new GUIContent("Pressed", null, string.Empty));
            rect.y += rect.height + 2;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("disabledTrigger"), new GUIContent("Disabled", null, string.Empty));
            rect.y += rect.height + 2;

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        #endregion

    }
}
