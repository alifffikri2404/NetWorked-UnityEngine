using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(SecondOrderRotation))]
    public class SecondOrderRotationEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SimpleProperty("target");
            SimpleProperty("frequency");
            SimpleProperty("damping");
            SimpleProperty("speed");

            MainContainerEnd();
        }

        #endregion

    }
}