using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(BroadcasterTerminal))]
    public class BroadcasterTerminalEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SimpleProperty("output");
            SimpleProperty("input");
            SimpleProperty("bufferSize");
            SimpleProperty("autoSizeContainer");
            SimpleProperty("sizePadding");
            SimpleProperty("scrollbar");

            MainContainerEnd();
        }

        #endregion

    }
}