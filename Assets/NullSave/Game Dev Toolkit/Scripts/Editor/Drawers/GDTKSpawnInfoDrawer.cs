using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    [CustomPropertyDrawer(typeof(GDTKSpawnInfo), true)]
    public class GDTKSpawnInfoDrawer : GDTKPropertyDrawer
    {

        #region Unity Methods

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int lines = 4;
            if (property.FindPropertyRelative("z_spawnError").boolValue) lines++;
            return (EditorGUIUtility.singleLineHeight + 2) * lines;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (fieldInfo.GetCustomAttributes(typeof(TooltipAttribute), true).FirstOrDefault() is TooltipAttribute tt) label.tooltip = tt.tooltip;

            Rect rect = position;
            rect.height = EditorGUIUtility.singleLineHeight;


            EditorGUI.BeginProperty(position, new GUIContent("Spawn Info"), property);

            EditorGUI.BeginChangeCheck();
            SimpleProperty(rect, property, "gameObject");
            rect.y += EditorGUIUtility.singleLineHeight + 2;
            if (EditorGUI.EndChangeCheck())
            {
                GameObject sprite = (GameObject)property.FindPropertyRelative("gameObject").objectReferenceValue;
                if (sprite == null)
                {
                    SimpleInt(property, "source", (int)SpawnSource.Resources);
                    SimpleString(property, "bundleName", null);
                    SimpleString(property, "path", null);
                    SimpleString(property, "assetName", null);
                    SimpleValue<bool>(property, "z_spawnError", false);
                }
                else
                {
                    string assetPath = AssetDatabase.GetAssetPath(sprite);
                    string bundlePath = string.IsNullOrEmpty(assetPath) ? string.Empty : AssetDatabase.GetImplicitAssetBundleName(assetPath);
                    if (!string.IsNullOrEmpty(bundlePath))
                    {
                        SimpleInt(property, "source", (int)SpawnSource.AssetBundle);
                        SimpleString(property, "bundleName", bundlePath);
                        SimpleString(property, "assetName", sprite.name);
                        SimpleString(property, "path", assetPath);
                        SimpleValue<bool>(property, "z_spawnError", false);
                    }
                    else if (!string.IsNullOrEmpty(assetPath))
                    {
                        if (assetPath.IndexOf("resources", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            SimpleInt(property, "source", (int)SpawnSource.Resources);
                            SimpleString(property, "path", Path.Combine(Path.GetDirectoryName(assetPath.Substring(assetPath.LastIndexOf("resources", StringComparison.OrdinalIgnoreCase) + 10)), sprite.name));
                            SimpleString(property, "bundleName", null);
                            SimpleString(property, "assetName", null);
                            SimpleValue<bool>(property, "z_spawnError", false);
                        }
                        else
                        {
                            SimpleValue<bool>(property, "z_spawnError", true);
                        }
                    }
                    else if (sprite == null)
                    {
                        SimpleInt(property, "source", (int)SpawnSource.Resources);
                        SimpleString(property, "path", null);
                        SimpleString(property, "bundleName", null);
                        SimpleString(property, "assetName", null);
                        SimpleValue<bool>(property, "z_spawnError", false);
                    }
                    else
                    {
                        SimpleValue<bool>(property, "z_spawnError", true);
                    }
                }
            }

            if (SimpleValue<bool>(property, "z_spawnError"))
            {
                EditorGUI.LabelField(rect, "Must be in Resources or an Asset Bundle", GDTKEditor.Styles.ErrorTextStyle);
                rect.y += EditorGUIUtility.singleLineHeight + 2;
            }

            SimpleProperty(rect, property, "parent");
            rect.y += EditorGUIUtility.singleLineHeight + 2;

            SimpleProperty(rect, property, "offset");
            rect.y += EditorGUIUtility.singleLineHeight + 2;

            EditorGUI.EndProperty();
        }


        #endregion

    }
}