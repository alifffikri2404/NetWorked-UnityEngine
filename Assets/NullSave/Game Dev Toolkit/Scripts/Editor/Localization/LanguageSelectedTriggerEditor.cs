using UnityEditor;

namespace NullSave.GDTK
{
    [CanEditMultipleObjects()]
    [CustomEditor(typeof(LanguageSelectedTrigger))]
    public class LanguageSelectedTriggerEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SubHeader("Behavior");
            SimpleProperty("language");

            SubHeader("Events");
            SimpleProperty("onMatch");
            SimpleProperty("onNoMatch");

            MainContainerEnd();
        }

        #endregion

    }
}