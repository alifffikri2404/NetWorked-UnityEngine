using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(Label))]
    public class LabelEditor : GDTKEditor
    {

        #region Fields

        private Label myTarget;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if(target is Label label)
            {
                myTarget = label;
            }
        }

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Behavior");
            string preText = serializedObject.FindProperty("m_Text").stringValue;
            SimpleProperty("m_Text");
            string text = SimpleValue<string>("m_Text");
            if(preText != text)
            {
                myTarget.text = text;
                EditorUtility.SetDirty(myTarget.target);
            }
            SimpleProperty("localize");
            SimpleProperty("autoSize");

#if ENABLE_INPUT_SYSTEM
            SectionHeader("Action Icons");
            SimpleProperty("useActionIcons");
            if(SimpleValue<bool>("useActionIcons"))
            {
                SimpleProperty("inputActions");
            }
#endif

            SectionHeader("Events");
            SimpleProperty("onTextChanged");
            SimpleProperty("onResized");

            MainContainerEnd();
        }

        #endregion

    }
}