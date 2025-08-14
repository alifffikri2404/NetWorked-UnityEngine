using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(FlexListItem))]
    [CanEditMultipleObjects]
    public class FlexListItemEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Behavior", GetIcon("icons/behavior"));
            SimpleProperty("image");
            SimpleProperty("label");
            SimpleProperty("background");
            SimpleProperty("m_colors");

            EditorGUILayout.Space(12);
            SectionHeader("Events", GetIcon("icons/event"));
            SimpleProperty("onClick");
            SimpleProperty("onSelected");
            SimpleProperty("onDeselected");


            MainContainerEnd();
        }

        #endregion

    }
}