using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(TriggerAfterTime))]
    public class TriggerAfterTimeEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Behavior");
            SimpleProperty("secondsToWait");
            SimpleProperty("repeat");

            SectionHeader("Events");
            SimpleProperty("onTimeElapsed");

            MainContainerEnd();
        }

        #endregion

    }
}