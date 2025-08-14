using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(UIMenu))]
    public class UIMenuEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Behavior", GetIcon("icons/behavior"));
            SimpleProperty("autoSelectFirstItem");

            GUILayout.Space(12);
            SectionHeader("Navigation", GetIcon("icons/navigation"));
            SimpleProperty("menuNav");

            GUILayout.Space(12);
            SectionHeader("Submit");
            SimpleProperty("submitButton");
            SimpleProperty("submitKey");

            GUILayout.Space(12);
            SectionHeader("Cancel");
            SimpleProperty("canCancel");
            if (SimpleValue<bool>("canCancel"))
            {
                SimpleProperty("cancelButton");
                SimpleProperty("cancelKey");
            }

            GUILayout.Space(12);
            SectionHeader("Audio", GetIcon("icons/audio"));
            SimpleProperty("audioSource");
            SimpleProperty("changeSelection");
            SimpleProperty("submit");
            SimpleProperty("cancel");

            GUILayout.Space(12);
            SectionHeader("Events", GetIcon("icons/event"));
            SimpleProperty("onSelectedIndexChanged");
            SimpleProperty("onSelectionChanged");
            SimpleProperty("onCancel");

            MainContainerEnd();
        }

        #endregion

    }
}