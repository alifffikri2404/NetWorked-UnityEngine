using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    [CustomUniversalPluginEditor(typeof(SpawnPrefabPlugin))]
    public class SpawnPrefabPluginEditor : UniversalPluginEditor
    {

        #region Unity Methods

        public override void OnEnable()
        {
            if(string.IsNullOrEmpty( PropertyStringValue("bundleName")))
            {
                if(!string.IsNullOrEmpty(PropertyStringValue("path")))
                {
                    // Resource
                    PropertyObjectValue("prefab", Resources.Load<GameObject>(PropertyStringValue("path")));
                }
            }
            else
            {
                // Bundle
                PropertyObjectValue("prefab", GetBundle().LoadAsset<GameObject>(PropertyStringValue("assetName")));

            }
        }

        public override void OnInspectorGUI()
        {
            object pre = PropertyObjectValue("prefab");
            PropertyField("prefab");
            object post = PropertyObjectValue("prefab");
            if (pre != post)
            {
                if (post == null)
                {
                    PropertyStringValue("path", string.Empty);
                    PropertyStringValue("bundleName", string.Empty);
                    PropertyStringValue("assetName", string.Empty);
                    PropertyBoolValue("z_spawnError", false);
                }
                else
                {
                    GameObject go = (GameObject)post;
                    string assetPath = AssetDatabase.GetAssetPath(go);
                    string bundlePath = string.IsNullOrEmpty(assetPath) ? string.Empty : AssetDatabase.GetImplicitAssetBundleName(assetPath);
                    if (!string.IsNullOrEmpty(bundlePath))
                    {
                        PropertyStringValue("bundleName", bundlePath);
                        PropertyStringValue("assetName", go.name);
                        PropertyStringValue("path", assetPath);
                        PropertyBoolValue("z_spawnError", false);
                    }
                    else if (!string.IsNullOrEmpty(assetPath))
                    {
                        if (assetPath.IndexOf("resources", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            PropertyStringValue("path", Path.Combine(Path.GetDirectoryName(assetPath.Substring(assetPath.LastIndexOf("resources", StringComparison.OrdinalIgnoreCase) + 10)), go.name));
                            PropertyStringValue("bundleName", string.Empty);
                            PropertyStringValue("assetName", string.Empty);
                            PropertyBoolValue("z_spawnError", false);
                        }
                        else
                        {
                            PropertyBoolValue("z_spawnError", true);
                        }
                    }
                    else
                    {
                        PropertyBoolValue("z_spawnError", true);
                    }
                }
                IsDirty = true;
            }
            if (PropertyBoolValue("z_spawnError"))
            {
                GUILayout.Label("Must be in Resources or an Asset Bundle", GDTKEditor.Styles.ErrorTextStyle);
            }
            PropertyField("offset");
            PropertyField("useRotation");
            PropertyField("parent");
        }

        #endregion

        #region Private Methods

        private AssetBundle GetBundle()
        {
            foreach (AssetBundle bundle in AssetBundle.GetAllLoadedAssetBundles())
            {
                if (bundle.name == PropertyStringValue("bundleName")) return bundle;
            }

            return AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath, PropertyStringValue("assetName")));
        }

        #endregion

    }
}