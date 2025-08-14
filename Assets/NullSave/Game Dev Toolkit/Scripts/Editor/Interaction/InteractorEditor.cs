using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(Interactor))]
    public class InteractorEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Behavior");
            SimpleProperty("interactionLayer");
            SimpleProperty("emissionSource");
            SimpleProperty("emissionOffset");
            SimpleProperty("maxDistance");

            SectionHeader("Broadcasting");
            SimpleProperty("broadcastEvents");
            if(SimpleValue<bool>("broadcastEvents"))
            {
                SimpleProperty("channelName");
            }

            SectionHeader("Registered Components");
            SimpleList(serializedObject.FindProperty("m_components"));

            SectionHeader("Events");
            SimpleProperty("onInteractableFound");
            SimpleProperty("onInteractableLost");

            MainContainerEnd();
        }

        #endregion

    }
}