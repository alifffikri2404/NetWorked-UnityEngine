using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(AudioPool))]
    public class AudioPoolEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SimpleProperty("channelName");
            SimpleProperty("template");

            MainContainerEnd();
        }

        #endregion

    }
}