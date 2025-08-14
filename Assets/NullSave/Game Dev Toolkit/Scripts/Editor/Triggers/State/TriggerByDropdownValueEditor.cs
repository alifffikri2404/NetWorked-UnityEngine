using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(TriggerByDropdownValue))]
    public class TriggerByDropdownValueEditor : GDTKEditor
    {

        #region Fields

        ReorderableList matchList;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            matchList = new ReorderableList(serializedObject, serializedObject.FindProperty("desiredValues"), true, true, true, true);
            matchList.elementHeight = EditorGUIUtility.singleLineHeight + 2;
            matchList.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "Desired Values"); };
            matchList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = matchList.serializedProperty.GetArrayElementAtIndex(index);

                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, new GUIContent("Index", null, string.Empty));
                rect.y += EditorGUIUtility.singleLineHeight + 2;
            };
        }

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Behavior");
            SimpleProperty("target");
            GUILayout.Space(8);
            matchList.DoLayoutList();

            SectionHeader("Events");
            SimpleProperty("onValueMatch");
            SimpleProperty("onValueMismatch");


            MainContainerEnd();
        }

        #endregion

    }
}