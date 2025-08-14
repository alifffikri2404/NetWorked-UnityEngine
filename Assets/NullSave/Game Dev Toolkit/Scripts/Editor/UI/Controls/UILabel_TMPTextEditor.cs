using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(UILabel_TMPText))]
    public class UILabel_TMPTextEditor : GDTKEditor
    {

        #region Fields

        private UILabel_TMPText myTarget;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if(target is UILabel_TMPText text)
            {
                myTarget = text;
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

            SectionHeader("Events");
            SimpleProperty("onTextChanged");
            SimpleProperty("onResized");

            MainContainerEnd();
        }

        #endregion

    }
}