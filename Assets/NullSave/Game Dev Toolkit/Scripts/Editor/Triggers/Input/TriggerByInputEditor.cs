using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(TriggerByInput))]
    [CanEditMultipleObjects]
    public class TriggerByInputEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Behavior", GetIcon("icons/behavior"));
            SimpleProperty("buttonName");
            SimpleProperty("keyCode");

            GUILayout.Space(12);
            SectionHeader("Events", GetIcon("icons/event"));
            SimpleProperty("onTriggered");

            MainContainerEnd();
        }

        #endregion

    }
}