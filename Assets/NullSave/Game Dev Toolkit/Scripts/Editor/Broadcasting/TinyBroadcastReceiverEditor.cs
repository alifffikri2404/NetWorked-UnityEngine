using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(TinyBroadcastReceiver))]
    public class TinyBroadcastReceiverEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            EditorGUI.BeginDisabledGroup(Application.isPlaying);

            SimpleProperty("usePublicChannel");
            if (!SimpleValue<bool>("usePublicChannel"))
            {
                SimpleProperty("channelName");
            }
            SimpleProperty("awaitMessage");

            SectionHeader("Events");
            SimpleProperty("onMessageReceived");


            EditorGUI.EndDisabledGroup();

            MainContainerEnd();
        }

        #endregion

    }
}