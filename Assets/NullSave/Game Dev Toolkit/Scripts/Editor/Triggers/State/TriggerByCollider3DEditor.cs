using UnityEditor;

namespace NullSave.GDTK
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(TriggerByCollider3D))]
    public class TriggerByCollider3DEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Behavior");
            SimpleProperty("layerMask");

            SectionHeader("Events");
            SimpleProperty("onEnter");
            SimpleProperty("onStay");
            SimpleProperty("onExit");

            MainContainerEnd();
        }

        #endregion

    }
}