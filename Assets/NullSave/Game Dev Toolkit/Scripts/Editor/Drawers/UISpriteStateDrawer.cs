using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    [CustomPropertyDrawer(typeof(UISpriteState))]
    public class UISpriteStateDrawer : PropertyDrawer
    {

        #region Public Methods

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (EditorGUIUtility.singleLineHeight + 2) * 4;
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
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("selectedSprite"), new GUIContent("Selected", null, string.Empty));
            rect.y += rect.height + 2;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("highlightedSprite"), new GUIContent("Highlighted", null, string.Empty));
            rect.y += rect.height + 2;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("pressedSprite"), new GUIContent("Pressed", null, string.Empty));
            rect.y += rect.height + 2;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("disabledSprite"), new GUIContent("Disabled", null, string.Empty));
            rect.y += rect.height + 2;

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        #endregion

    }
}
