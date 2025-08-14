using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(ObjectPullSource))]
    public class ObjectPullSourceEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Behavior");
            SimpleProperty("affectedLayers");
            SimpleProperty("pullRadius");
            SimpleProperty("delayBeforePull");
            SimpleProperty("pullDuration");
            SimpleProperty("pullToOffset");
            SimpleProperty("destroyAfterPull");

            MainContainerEnd();
        }

        #endregion

    }
}