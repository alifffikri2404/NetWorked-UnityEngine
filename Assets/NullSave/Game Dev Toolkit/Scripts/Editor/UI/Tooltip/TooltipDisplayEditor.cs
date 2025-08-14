using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(TooltipDisplay))]
    public class TooltipDisplayEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SimpleProperty("tipText");
            SimpleProperty("maxSize");
            SimpleProperty("padding");

            MainContainerEnd();
        }

        #endregion

    }
}