using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(Broadcaster))]
    public class BroadcasterEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            GUILayout.BeginVertical("box");
            GUILayout.Label("The GDTK Broadcaster enables you to send messages to receivers either publicly or over named channels. The broadcaster itself has no configurable settings and there should only be one per scene.", Styles.WrappedTextStyle);
            GUILayout.EndVertical();

            MainContainerEnd();
        }

        #endregion

    }
}