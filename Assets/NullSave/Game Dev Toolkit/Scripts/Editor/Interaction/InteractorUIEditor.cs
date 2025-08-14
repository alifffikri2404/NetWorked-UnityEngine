using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(InteractorUI))]
    public class InteractorUIEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SimpleProperty("uiText", "UI Text");
            SimpleProperty("format", "Text Format");
            SimpleProperty("requireHold");
            if(SimpleValue<bool>("requireHold"))
            {
                SimpleProperty("holdTime");
                SimpleProperty("holdProgressbar");

                EditorGUILayout.Space(12);
                SimpleProperty("onHeldTimeChanged");
            }

            MainContainerEnd();
        }

        #endregion

    }
}