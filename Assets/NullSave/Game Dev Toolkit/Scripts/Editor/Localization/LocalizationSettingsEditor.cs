using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(LocalizationSettings))]
    public class LocalizationSettingsEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SimpleProperty("sourceType");
            switch((DictionarySource)SimpleValue<int>("sourceType"))
            {
                case DictionarySource.ResourceFile:
                    SimpleProperty("filename");
                    break;
                case DictionarySource.AssetBundle:
                    SimpleProperty("bundleName");
                    SimpleProperty("bundleAssetName");
                    SimpleProperty("bundleUrl");
                    break;
            }
            SimpleProperty("lookupMode");
            SimpleProperty("encoding");
            SimpleProperty("defaultLanguage");

            MainContainerEnd();
        }

        #endregion

    }
}
