using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(LockOnTarget))]
    public class LockOnTargetEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SimpleProperty("indicatorOffset");

            MainContainerEnd();
        }

        #endregion

    }
}
