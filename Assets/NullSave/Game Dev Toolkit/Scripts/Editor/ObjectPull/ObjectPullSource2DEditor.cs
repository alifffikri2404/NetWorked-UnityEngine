using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(ObjectPullSource2D))]
    public class ObjectPullSource2DEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Behavior");
            SimpleProperty("affectedLayers");
            SimpleProperty("delayBeforePull");
            SimpleProperty("pullRadius");
            SimpleProperty("pullDuration");
            SimpleProperty("pullToOffset");
            SimpleProperty("destroyAfterPull");

            MainContainerEnd();
        }

        #endregion

    }
}