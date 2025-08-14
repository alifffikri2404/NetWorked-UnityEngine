using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(DestroyAfterTime))]
    public class DestroyAfterTimeEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Behavior");
            SimpleProperty("secondsToLive");

            MainContainerEnd();
        }

        #endregion

    }
}