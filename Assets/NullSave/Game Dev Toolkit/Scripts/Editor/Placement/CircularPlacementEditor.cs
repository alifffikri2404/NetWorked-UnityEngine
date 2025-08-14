using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(CircularPlacement))]
    public class CircularPlacementEditor : GDTKEditor
    {

        #region Fields

        ReorderableList ringsList;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            ringsList = new ReorderableList(serializedObject, serializedObject.FindProperty("rings"), true, true, true, true);
            ringsList.elementHeight = (EditorGUIUtility.singleLineHeight + 2) * 2;
            ringsList.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "Placement Rings"); };
            ringsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = ringsList.serializedProperty.GetArrayElementAtIndex(index);

                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("distanceFromCenter"), new GUIContent("Units from Center", null, string.Empty));
                rect.y += EditorGUIUtility.singleLineHeight + 2;
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("degreesPerStep"), new GUIContent("Step Degrees", null, string.Empty));
                rect.y += EditorGUIUtility.singleLineHeight + 2;
            };
        }

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Behavior");
            SimpleProperty("placementRadius");
            SimpleProperty("placementOffset");
            SimpleProperty("placementMask");

            GUILayout.Space(4);
            ringsList.DoLayoutList();

            SectionHeader("Editor");
            SimpleProperty("drawGizmos");

            MainContainerEnd();
        }

        #endregion

    }
}