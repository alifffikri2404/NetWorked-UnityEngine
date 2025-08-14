using UnityEditor;

namespace NullSave.GDTK
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(FocusScaleUIElement))]
    public class FocusScaleUIElementEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SimpleProperty("target");
            SimpleProperty("defaultScale");
            SimpleProperty("hoverScale");
            SimpleProperty("transitionTime");

            MainContainerEnd();
        }

        #endregion

    }
}
