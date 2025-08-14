using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    [CustomPropertyDrawer(typeof(CircularPlacementRing))]
    public class CircularPlacementRingDrawer : PropertyDrawer
    {

        #region Public Methods

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (EditorGUIUtility.singleLineHeight + 2) * 2;
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
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("distanceFromCenter"), new GUIContent("Units from Center", null, string.Empty));
            rect.y += rect.height + 2;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("degreesPerStep"), new GUIContent("Step Degrees", null, string.Empty));
            rect.y += rect.height + 2;

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        #endregion

    }
}
