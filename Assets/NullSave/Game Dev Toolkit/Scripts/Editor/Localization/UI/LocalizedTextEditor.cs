using UnityEditor;

namespace NullSave.GDTK
{
    [CanEditMultipleObjects()]
    [CustomEditor(typeof(LocalizedText))]
    public class LocalizedTextEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            DrawPropertiesExcluding(serializedObject, new string[] { "m_Script" });

            MainContainerEnd();
        }

        #endregion

    }
}