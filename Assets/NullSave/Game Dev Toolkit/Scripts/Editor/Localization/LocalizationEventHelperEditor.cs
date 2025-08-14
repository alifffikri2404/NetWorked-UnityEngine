using UnityEditor;

namespace NullSave.GDTK
{
    [CanEditMultipleObjects()]
    [CustomEditor(typeof(LocalizationEventHelper))]
    public class LocalizationEventHelperEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SubHeader("Behavior");
            SimpleProperty("downloadWindow");
            SimpleProperty("downloadProgress");

            SubHeader("Events");
            SimpleProperty("onSourceIsLocal");
            SimpleProperty("onSourceIsDLC");
            SimpleProperty("onLoadIsMemory");
            SimpleProperty("onLoadIsRealTime");

            MainContainerEnd();
        }

        #endregion

    }
}