#if ENABLE_INPUT_SYSTEM
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(ActionIcons))]
    public class ActionIconsEditor : GDTKEditor
    {

        #region Fields

        ReorderableList mapsList;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            mapsList = new ReorderableList(serializedObject, serializedObject.FindProperty("controllerMaps"), true, false, true, true);
            mapsList.elementHeight = EditorGUIUtility.singleLineHeight + 2;
            mapsList.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "Controller Maps"); };
            mapsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = mapsList.serializedProperty.GetArrayElementAtIndex(index);

                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element);
                rect.y += EditorGUIUtility.singleLineHeight + 2;
            };
        }

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Controller Maps", GetIcon("icons/book"));
            mapsList.DoLayoutList();

            GUILayout.Space(12);
            SectionHeader("Events", GetIcon("icons/event"));
            SimpleProperty("onControllerChanged");

            MainContainerEnd();
        }

        #endregion

    }
}
#endif