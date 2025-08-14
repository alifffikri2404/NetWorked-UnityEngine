using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(InteractableDoor))]
    public class InteractableDoorEditor : GDTKEditor
    {

        #region Fields

        private InteractableDoor myTarget;

        #endregion

        #region Unity Methods

        public void OnEnable()
        {
            if (target is InteractableDoor)
            {
                myTarget = target as InteractableDoor;
            }
        }

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Behavior");
            SimpleProperty("interactable");
            SimpleProperty("openBoolAnim");

            myTarget.IsOpen = EditorGUILayout.Toggle("Is Open", myTarget.IsOpen);

            SimpleProperty("openText");
            SimpleProperty("closeText");
            SimpleProperty("customUI");
            if (SimpleValue<bool>("showAltText"))
            {
                SimpleProperty("alternateText");
            }


            SectionHeader("Audio");
            SimpleProperty("audioPoolChannel");
            SimpleProperty("openSound");
            SimpleProperty("closeSound");

            SectionHeader("Events");
            SimpleProperty("onOpen");
            SimpleProperty("onClose");

            MainContainerEnd();
        }

        #endregion

    }
}