using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(TriggerByButton))]
    public class TriggerByButtonEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Behavior");
            SimpleProperty("button");
            SimpleProperty("onButton");
            SimpleProperty("onButtonDown");
            SimpleProperty("onButtonUp");

            SectionHeader("Events");
            SimpleProperty("onTrigger");

            MainContainerEnd();
        }

        #endregion

    }
}