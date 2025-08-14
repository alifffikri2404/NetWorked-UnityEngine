using UnityEngine;
using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(DestroyOnBroadcastMessage))]
    public class DestroyOnBroadcastMessageEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            GUILayout.BeginVertical("box");
            GUILayout.Label("This component destroys the attached GameObject when a specific message is received by a GDTK Broadcaster.", Styles.WrappedTextStyle);
            GUILayout.EndVertical();


            EditorGUI.BeginDisabledGroup(Application.isPlaying);

            SimpleProperty("destroyMessage");
            SimpleProperty("usePublicChannel");
            if(!SimpleValue<bool>("usePublicChannel"))
            {
                SimpleProperty("channelName");
            }

            EditorGUI.EndDisabledGroup();

            MainContainerEnd();
        }

        #endregion

    }
}