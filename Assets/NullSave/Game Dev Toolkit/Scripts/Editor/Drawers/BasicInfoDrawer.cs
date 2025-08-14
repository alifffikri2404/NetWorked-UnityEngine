using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    [CustomPropertyDrawer(typeof(BasicInfo), true)]
    public class BasicInfoDrawer : GDTKPropertyDrawer
    {

        #region Fields

        private Vector2 scrollPos;

        #endregion

        #region Unity Methods

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int lines = 12 + property.FindPropertyRelative("tags").arraySize;
            string id = SimpleString(property, "id");
            if (property.FindPropertyRelative("image").FindPropertyRelative("z_imageError").boolValue) lines++;
            if (string.IsNullOrEmpty(id) || !id.IsAllowedId()) lines++;
            return 26 + ((EditorGUIUtility.singleLineHeight + 2) * lines);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (fieldInfo.GetCustomAttributes(typeof(TooltipAttribute), true).FirstOrDefault() is TooltipAttribute tt) label.tooltip = tt.tooltip;

            string id = SimpleString(property, "id");
            bool autoGen = SimpleBool(property, "autoGenId");
            Rect rect = position;
            rect.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.BeginProperty(position, label, property);

            SimpleProperty(rect, property, "autoGenId");
            rect.y += EditorGUIUtility.singleLineHeight + 2;

            EditorGUI.BeginDisabledGroup(autoGen);
            Rect r2 = rect;
            r2.width -= 32;
            SimpleProperty(r2, property, "id");


            EditorGUI.EndDisabledGroup();

            r2.x += r2.width + 2;
            r2.width = 30;
            if(GUI.Button(r2, new GUIContent(GDTKEditor.GetIcon("icons/copy"), "Copy")))
            {
                GUIUtility.systemCopyBuffer = property.FindPropertyRelative("id").stringValue;
            }
            
            rect.y += EditorGUIUtility.singleLineHeight + 2;

            if (string.IsNullOrEmpty(id))
            {
                EditorGUI.LabelField(rect, "Id cannot be empty", GDTKEditor.Styles.ErrorTextStyle);
                rect.y += EditorGUIUtility.singleLineHeight + 2;
            }
            else if (!id.IsAllowedId())
            {
                EditorGUI.LabelField(rect, "Id must only contain letters, numbers, and underscore", GDTKEditor.Styles.ErrorTextStyle);
                rect.y += EditorGUIUtility.singleLineHeight + 2;
            }
            else
            {
                EditorGUI.LabelField(rect, string.Empty);
            }

            GUI.SetNextControlName("txtTitle");
            SimpleProperty(rect, property, "title");
            rect.y += EditorGUIUtility.singleLineHeight + 2;

            if (autoGen)
            {
                SimpleString(property, "id", SimpleString(property, "title").ToAllowedId());
            }

            SimpleProperty(rect, property, "abbr");
            rect.y += EditorGUIUtility.singleLineHeight + 2;

            EditorGUI.LabelField(rect, "Description");

            r2.x = rect.x + EditorGUIUtility.labelWidth + 2;
            r2.y = rect.y;
            r2.width = rect.width - EditorGUIUtility.labelWidth - 2;
            r2.height = rect.height * 3;
            SimpleString(property, "description", EditorGUI.TextArea(r2, SimpleString(property, "description")));

            //scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(r2.height));
            //EditorGUILayout.TextArea(SimpleString(property, "description"));
            //EditorGUILayout.EndScrollView();

            rect.y += (EditorGUIUtility.singleLineHeight + 2) * 3;

            SimpleProperty(rect, property, "groupName");
            rect.y += EditorGUIUtility.singleLineHeight + 2;

            EditorGUI.BeginChangeCheck();
            SerializedProperty image = property.FindPropertyRelative("image");
            SimpleProperty(rect, image, "sprite");
            rect.y += EditorGUIUtility.singleLineHeight + 2;
            if (EditorGUI.EndChangeCheck())
            {
                Sprite sprite = (Sprite)image.FindPropertyRelative("sprite").objectReferenceValue;
                if (sprite == null)
                {
                    SimpleInt(image, "source", (int)ImageSource.None);
                    SimpleString(image, "bundleName", null);
                    SimpleString(image, "path", null);
                    SimpleString(image, "assetName", null);
                    SimpleBool(image, "z_imageError", false);
                }
                else
                {
                    string assetPath = AssetDatabase.GetAssetPath(sprite);
                    string bundlePath = string.IsNullOrEmpty(assetPath) ? string.Empty : AssetDatabase.GetImplicitAssetBundleName(assetPath);
                    if (!string.IsNullOrEmpty(bundlePath))
                    {
                        SimpleInt(image, "source", (int)ImageSource.AssetBundle);
                        SimpleString(image, "bundleName", bundlePath);
                        SimpleString(image, "assetName", sprite.name);
                        SimpleString(image, "path", assetPath);
                        SimpleBool(image, "z_imageError", false);
                    }
                    else if (!string.IsNullOrEmpty(assetPath))
                    {
                        if (assetPath.IndexOf("resources", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            SimpleInt(image, "source", (int)ImageSource.Resources);
                            SimpleString(image, "path", Path.Combine(Path.GetDirectoryName(assetPath.Substring(assetPath.LastIndexOf("resources", StringComparison.OrdinalIgnoreCase) + 10)), sprite.name));
                            SimpleString(image, "bundleName", null);
                            SimpleString(image, "assetName", null);
                            SimpleBool(image, "z_imageError", false);
                        }
                        else
                        {
                            SimpleBool(image, "z_imageError", true);
                        }
                    }
                    else if (sprite == null)
                    {
                        SimpleInt(image, "source", (int)ImageSource.None);
                        SimpleString(image, "path", null);
                        SimpleString(image, "bundleName", null);
                        SimpleString(image, "assetName", null);
                        SimpleBool(image, "z_imageError", false);
                    }
                    else
                    {
                        SimpleBool(image, "z_imageError", true);
                    }
                }
            }

            if (SimpleBool(image, "z_imageError"))
            {
                EditorGUI.LabelField(rect, "Must be in Resources or an Asset Bundle", GDTKEditor.Styles.ErrorTextStyle);
                rect.y += EditorGUIUtility.singleLineHeight + 2;
            }

            SimpleProperty(rect, property, "color");
            rect.y += EditorGUIUtility.singleLineHeight + 2;


            SerializedProperty list = property.FindPropertyRelative("tags");
            SerializedProperty entry;
            int removeAt = -1;

            EditorGUI.LabelField(rect, "Tags", "(" + list.arraySize + ")");
            rect.y += EditorGUIUtility.singleLineHeight + 2;

            for (int i = 0; i < list.arraySize; i++)
            {
                entry = list.GetArrayElementAtIndex(i);
                r2.x = rect.x + EditorGUIUtility.labelWidth + 4;
                r2.y = rect.y;
                r2.width = rect.width - EditorGUIUtility.labelWidth - 36;
                r2.height = rect.height;

                entry.stringValue = EditorGUI.TextField(r2, entry.stringValue);

                r2.x += r2.width + 2;
                r2.width = 32;
                if (GUI.Button(r2, GDTKEditor.GetIcon("icons/trash-small")))
                {
                    GUI.FocusControl(null);
                    removeAt = i;
                }

                rect.y += EditorGUIUtility.singleLineHeight + 2;
            }

            r2.x = rect.x + EditorGUIUtility.labelWidth + 2;
            r2.y = rect.y;
            r2.width = 100;
            r2.height = 24;

            if (GUI.Button(r2, new GUIContent("Add New", GDTKEditor.GetIcon("icons/add-small"))))
            {
                list.arraySize++;
                list.GetArrayElementAtIndex(list.arraySize - 1).stringValue = string.Empty;
                GUI.FocusControl(null);
            }
            rect.y += 26;

            RemoveFromList(list, removeAt);


            SimpleProperty(rect, property, "hidden");
            rect.y += EditorGUIUtility.singleLineHeight + 2;

            EditorGUI.EndProperty();
        }


        #endregion

        #region Private Methods

        private void RemoveFromList(SerializedProperty list, int index)
        {
            if (index < 0) return;
            int preSize = list.arraySize;
            list.DeleteArrayElementAtIndex(index);
            if (preSize == list.arraySize)
            {
                list.DeleteArrayElementAtIndex(index);
            }
        }

        #endregion

    }
}