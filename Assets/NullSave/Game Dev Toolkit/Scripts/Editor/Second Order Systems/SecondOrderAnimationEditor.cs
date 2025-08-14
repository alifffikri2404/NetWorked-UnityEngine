using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(SecondOrderAnimation))]
    public class SecondOrderAnimationEditor : GDTKEditor
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