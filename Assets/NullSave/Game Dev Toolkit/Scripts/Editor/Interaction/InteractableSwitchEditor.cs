using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(InteractableSwitch))]
    public class InteractableSwitchEdior : GDTKEditor
    {

        #region Fields

        private InteractableSwitch myTarget;

        #endregion

        #region Unity Methods

        public void OnEnable()
        {
            if(target is InteractableSwitch)
            {
                myTarget = target as InteractableSwitch;
            }
        }

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Behavior");
            SimpleProperty("interactable");
            SimpleProperty("onBoolAnim");

            myTarget.IsOn = EditorGUILayout.Toggle("Is On", myTarget.IsOn);

            SimpleProperty("activateText");
            SimpleProperty("deactivateText");
            SimpleProperty("customUI");
            if (SimpleValue<bool>("showAltText"))
            {
                SimpleProperty("alternateText");
            }

            SectionHeader("Audio");
            SimpleProperty("audioPoolChannel");
            SimpleProperty("actionSound");

            SectionHeader("Events");
            SimpleProperty("onSwitchOn");
            SimpleProperty("onSwitchOff");

            MainContainerEnd();
        }

        #endregion

    }
}