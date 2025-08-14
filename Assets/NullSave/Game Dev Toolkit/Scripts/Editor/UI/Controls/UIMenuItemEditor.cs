using TMPro;
using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UIMenuItem), true)]
    public class UIMenuItemEditor : GDTKEditor
    {

        #region Fields

        private UIMenuItem myTarget;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if(target is UIMenuItem item)
            {
                myTarget = item;
            }
        }

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Behaviour", GetIcon("icons/behavior"));
            SimpleProperty("m_interactable");

            GUILayout.Space(12);
            SectionHeader("Background", GetIcon("icons/ui"));
            SimpleProperty("transition");
            EditorGUI.indentLevel++;
            switch (SimpleValue<int>("transition"))
            {
                case 1: // Color Tint
                    SimpleProperty("background");
                    SimpleProperty("m_backgroundColors");
                    break;
                case 2: // Sprite Swap
                    SimpleProperty("background");
                    SimpleProperty("spriteState");
                    break;
                case 3: // Animation
                    SimpleProperty("background");
                    SimpleProperty("animationTriggers");
                    break;
            }
            EditorGUI.indentLevel--;

            GUILayout.Space(12);
            SectionHeader("Text", GetIcon("icons/code-small"));
            SimpleProperty("textLabel");
            myTarget.text = EditorGUILayout.TextField("Text", myTarget.text);
            SimpleProperty("colorText");
            if(SimpleValue<bool>("colorText"))
            {
                SimpleProperty("m_fontColors");
            }

            GUILayout.Space(12);
            SectionHeader("Events", GetIcon("icons/event"));
            SimpleProperty("onSelected");
            SimpleProperty("onDeselected");
            SimpleProperty("onSubmit");

            MainContainerEnd();
        }

        #endregion

    }
}
