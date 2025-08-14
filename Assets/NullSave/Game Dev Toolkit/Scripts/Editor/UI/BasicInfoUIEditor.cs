using UnityEditor;

namespace NullSave.GDTK
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(BasicInfoUI))]
    public class BasicInfoUIEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SimpleProperty("id");
            SimpleProperty("title");
            SimpleProperty("abbreviation");
            SimpleProperty("description");
            SimpleProperty("groupName");
            SimpleProperty("image");

            MainContainerEnd();
        }

        #endregion

    }
}