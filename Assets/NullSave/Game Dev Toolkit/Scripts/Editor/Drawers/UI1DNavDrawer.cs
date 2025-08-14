using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK.Inventory
{
    [CustomPropertyDrawer(typeof(UI1DNav))]
    public class UI1DNavDrawer : GDTKPropertyDrawer
    {

        #region Public Methods

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float lineSize = EditorGUIUtility.singleLineHeight + 2;
            int lineCount = 3;

            InputFlags flags = (InputFlags)SimpleInt(property, "allowedInput");

            // Check for repeat inputs
            if (flags.HasFlag(InputFlags.Axis) || flags.HasFlag(InputFlags.Key))
            {
                lineCount += 2;
                if (SimpleValue<bool>(property, "autoRepeat")) lineCount++;
            }

            if (flags.HasFlag(InputFlags.Axis))
            {
                lineCount++;
            }

            if (flags.HasFlag(InputFlags.Key))
            {
                lineCount += 2;
            }

            return lineCount * lineSize;
        }

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            EditorGUI.indentLevel++;

            var rect = position;
            rect.height = EditorGUIUtility.singleLineHeight + 2;
            rect.y += rect.height;

            InputFlags flags = (InputFlags)SimpleInt(property, "allowedInput");
            SimpleEnumFlagsContext(rect, property.FindPropertyRelative("allowedInput"), typeof(InputFlags));
            rect.y += rect.height;

            if (flags.HasFlag(InputFlags.Axis))
            {
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("navAxis"));
                rect.y += rect.height;
            }

            if (flags.HasFlag(InputFlags.Key))
            {
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("backKey"));
                rect.y += rect.height;
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("nextKey"));
                rect.y += rect.height;
            }

            if (flags.HasFlag(InputFlags.Axis) || flags.HasFlag(InputFlags.Key))
            {
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("autoRepeat"));
                rect.y += rect.height;
                if (property.FindPropertyRelative("autoRepeat").boolValue)
                {
                    EditorGUI.PropertyField(rect, property.FindPropertyRelative("repeatDelay"));
                    rect.y += rect.height;
                }
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("allowAutoWrap"));
                rect.y += rect.height;
            }

            EditorGUI.PropertyField(rect, property.FindPropertyRelative("lockInput"));
            rect.y += rect.height;

            EditorGUI.indentLevel--;

            EditorGUI.EndProperty();
        }

        #endregion

    }
}
