using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(SliderInput))]
    public class SliderInputEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SimpleProperty("slideAxis");
            SimpleProperty("sensitivity");
            SimpleProperty("integerValues");
            if (SimpleValue<bool>("integerValues"))
            {
                SimpleProperty("repeatDelay");
            }

            MainContainerEnd();
        }

        #endregion

    }
}