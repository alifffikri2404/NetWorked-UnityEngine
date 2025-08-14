using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(TooltipClient))]
    public class TooltipClientEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Behavior");
            SimpleProperty("dynamicText");
            if (SimpleValue<bool>("dynamicText"))
            {
                SimpleProperty("textSource");
            }
            else
            {
                SimpleProperty("tooltip");
            }
            SimpleProperty("customTooltip");
            SimpleProperty("modifyDelay");

            SectionHeader("Events");
            SimpleProperty("onDisplay");
            SimpleProperty("onHide");

            MainContainerEnd();
        }

        #endregion

    }
}