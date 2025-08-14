using UnityEditor;

namespace NullSave.GDTK
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ObjectPullTarget))]
    public class ObjectPullTargetEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Behavior");
            SimpleProperty("additionalDelay");
            SimpleProperty("additionalDuration");

            MainContainerEnd();
        }

        #endregion

    }
}