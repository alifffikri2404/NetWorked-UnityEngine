using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(TriggerByToggleState))]
    public class TriggerByToggleStateEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Events");
            SimpleProperty("onValueTrue");
            SimpleProperty("onValueFalse");

            MainContainerEnd();
        }

        #endregion

    }
}