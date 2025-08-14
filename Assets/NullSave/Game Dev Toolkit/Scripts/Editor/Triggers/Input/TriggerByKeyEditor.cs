using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(TriggerByKey))]
    public class TriggerByKeyEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Behavior");
            SimpleProperty("key");
            SimpleProperty("onKey");
            SimpleProperty("onKeyDown");
            SimpleProperty("onKeyUp");

            SectionHeader("Events");
            SimpleProperty("onTrigger");

            MainContainerEnd();
        }

        #endregion

    }
}