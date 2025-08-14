using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(InteractableObject))]
    public class InteractableObjectEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Behavior");
            SimpleProperty("interactable");
            SimpleProperty("actionText");
            SimpleProperty("customUI");
            SimpleProperty("showAltText");
            if(SimpleValue<bool>("showAltText"))
            {
                SimpleProperty("alternateText");
            }

            SectionHeader("Audio");
            SimpleProperty("audioPoolChannel");
            SimpleProperty("actionSound");

            SectionHeader("Events");
            SimpleProperty("onInteract");

            MainContainerEnd();
        }

        #endregion

    }
}