using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(ColorTransition))]
    public class ColorTransitionEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SimpleProperty("m_colors");

            MainContainerEnd();
        }

        #endregion

    }
}