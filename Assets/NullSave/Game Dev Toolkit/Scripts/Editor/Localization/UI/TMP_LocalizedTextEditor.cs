using UnityEditor;

namespace NullSave.GDTK
{
    [CanEditMultipleObjects()]
    [CustomEditor(typeof(TMP_LocalizedText))]
    public class TMP_LocalizedTextEditor : GDTKEditor
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