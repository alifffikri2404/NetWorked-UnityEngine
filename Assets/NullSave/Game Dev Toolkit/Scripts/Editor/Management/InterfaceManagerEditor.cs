using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(InterfaceManager))]
    public class InterfaceManagerEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("General");
            SimpleProperty("persist");
            SimpleProperty("inputManager");
            SimpleProperty("objectManager");
            SimpleProperty("m_localizationSettings");

            SectionHeader("UI Canvas");
            SimpleProperty("uiScaleMode", "Scale Mode");
            switch ((UIScaleMode)SimpleValue<int>("uiScaleMode"))
            {
                case UIScaleMode.ConstantPhysicalSize:
                    SimpleProperty("physicalUnit");
                    SimpleProperty("fallbackScreenDPI");
                    SimpleProperty("defaultSpriteDPI");
                    SimpleProperty("referencePixelsPerUnit");
                    break;
                case UIScaleMode.ConstantPixelSize:
                    SimpleProperty("scaleFactor");
                    SimpleProperty("referencePixelsPerUnit");
                    break;
                case UIScaleMode.ScaleWithScreenSize:
                    SimpleProperty("referenceResolution");
                    SimpleProperty("screenMatchMode");
                    Rect r = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight + 12);
                    DualLabeledSlider(r, serializedObject.FindProperty("matchWidthOrHeight"), new GUIContent("Match"), new GUIContent("Width"), new GUIContent("Height"));
                    SimpleProperty("referencePixelsPerUnit");
                    break;
            }
            SimpleProperty("includeRaycaster");

            SectionHeader("Interaction");
            SimpleProperty("interactorPrefab");
            SimpleProperty("interactionType");
            switch ((NavigationTypeSimple)SimpleValue<int>("interactionType"))
            {
                case NavigationTypeSimple.ByButton:
                    SimpleProperty("interactionButton");
                    break;
                case NavigationTypeSimple.ByKey:
                    SimpleProperty("interactionKey");
                    break;
            }

            SectionHeader("Tooltip");
            SimpleProperty("tooltipPrefab");
            SimpleProperty("tipOffset");
            SimpleProperty("displayDelay");

            SectionHeader("Tab Stops");
            SimpleProperty("activateFirstTabStop");
            SimpleProperty("tabStyle");
            switch ((NavigationTypeSimple)SimpleValue<int>("tabStyle"))
            {
                case NavigationTypeSimple.ByButton:
                    SimpleProperty("tabButton");
                    break;
                case NavigationTypeSimple.ByKey:
                    SimpleProperty("tabKey");
                    break;
            }

            MainContainerEnd();
        }

        #endregion

    }
}