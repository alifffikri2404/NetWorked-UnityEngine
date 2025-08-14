using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(DebugConsole))]
    public class DebugConsoleEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Behavior");
            SimpleProperty("showHide");
            switch ((NavigationTypeSimple)SimpleValue<int>("showHide"))
            {
                case NavigationTypeSimple.ByButton:
                    SimpleProperty("showHideButton", "Button");
                    break;
                case NavigationTypeSimple.ByKey:
                    SimpleProperty("showHideKey", "Key");
                    break;
            }
            SimpleProperty("startShown");
            SimpleProperty("showHideAnimBool");

            SectionHeader("UI");
            SimpleProperty("output");
            SimpleProperty("input");
            SimpleProperty("bufferSize");
            SimpleProperty("autoSizeContainer");
            SimpleProperty("sizePadding");
            SimpleProperty("scrollbar");

            SectionHeader("Events");
            SimpleProperty("onShow");
            SimpleProperty("onHide");


            MainContainerEnd();
        }

        #endregion

    }
}