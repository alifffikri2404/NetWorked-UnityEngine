#if ENABLE_INPUT_SYSTEM
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(ControllerMap))]
    public class ControllerMapEditor : GDTKEditor
    {

        #region Variables

        private Dictionary<string, string> controllers;
        private Vector2 compatScroll;
        private string contollerSearch;
        private ControllerMap myTarget;
        private EditorInfoList inputList;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if (target is ControllerMap map)
            {
                myTarget = map;
            }

            // Get Gamepad Types
            controllers = new Dictionary<string, string>();
            Type inputDevice = typeof(InputDevice);
            Type[] types;
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in assemblies)
            {
                types = asm.GetTypes();
                foreach (Type type in types)
                {
                    if (type.BaseType != null && inputDevice.IsAssignableFrom(type))
                    {
                        controllers.Add(ObjectNames.NicifyVariableName(type.Name), type.Name);
                    }
                }
            }
            controllers = controllers.Keys.OrderBy(k => k).ToDictionary(k => k, k => controllers[k]);

            inputList = new EditorInfoList();
        }

        public override void OnInspectorGUI()
        {
            bool wasPresent, isPresent;

            if (myTarget.compatibleDevices == null) myTarget.compatibleDevices = new List<string>();
            if (myTarget.inputMaps == null) myTarget.inputMaps = new List<InputMap>();

            MainContainerBegin();

            SectionHeader("Behavior");
            SimpleProperty("isFallback");
            SimpleProperty("tmpSpriteAsset", "TMPro Sprite Asset");


            SectionHeader("Compatible Devices");
            contollerSearch = SearchBox(contollerSearch);

            GUILayout.Space(-4);
            compatScroll = GUILayout.BeginScrollView(compatScroll, "box", GUILayout.MaxHeight(289), GUILayout.MinHeight(100));
            foreach (KeyValuePair<string, string> entry in controllers)
            {
                if (string.IsNullOrWhiteSpace(contollerSearch) || entry.Key.IndexOf(contollerSearch, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    wasPresent = myTarget.compatibleDevices.Contains(entry.Value);
                    GUILayout.BeginHorizontal();
                    isPresent = GUILayout.Toggle(wasPresent, string.Empty);
                    GUILayout.Label(entry.Key);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    if (wasPresent != isPresent)
                    {
                        if (isPresent)
                        {
                            myTarget.compatibleDevices.Add(entry.Value);
                        }
                        else
                        {
                            myTarget.compatibleDevices.Remove(entry.Value);
                        }
                    }
                }
            }
            GUILayout.EndScrollView();

            SectionHeader("Input Maps");
            int delIndex = SimpleObjectDragList(serializedObject.FindProperty("inputMaps"), "inputName", inputList, DrawInputMap, true);
            if (delIndex > -1)
            {
                myTarget.inputMaps.RemoveAt(delIndex);
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add", Styles.ButtonLeft))
            {
                myTarget.inputMaps.Add(new InputMap() { inputName = "Input" });
            }
            if (GUILayout.Button("Clear", Styles.ButtonRight))
            {
                if (EditorUtility.DisplayDialog("GDTK", "Are you sure you wish to remove all input maps from this controller?", "Yes", "Cancel"))
                {
                    myTarget.inputMaps.Clear();
                }
            }
            GUILayout.EndHorizontal();

            MainContainerEnd();
        }

        public override bool RequiresConstantRepaint()
        {
            if (inputList == null) return false;
            return inputList.isDragging;
        }

        #endregion

        #region Private Methods

        private void DrawInputMap(SerializedProperty item, int index)
        {
            SimpleProperty(item, "inputName");

            if (myTarget.tmpSpriteAsset == null)
            {
                EditorGUI.BeginDisabledGroup(true);
                SimpleProperty(item, "tmpSpriteIndex", "Sprite Index");
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                if (myTarget.tmpSpriteAsset.spriteInfoList == null || myTarget.tmpSpriteAsset.spriteInfoList.Count == 0)
                {
                    PropertyInfo spriteGlyphTable = myTarget.tmpSpriteAsset.GetType().GetProperty("spriteGlyphTable");
                    if (spriteGlyphTable != null)
                    {
                        object list = spriteGlyphTable.GetValue(myTarget.tmpSpriteAsset);
                        SimpleValue(item, "tmpSpriteIndex", EditorGUILayout.IntSlider("Sprite Index", SimpleValue<int>(item, "tmpSpriteIndex"), 0, (int)list.GetType().GetProperty("Count").GetValue(list) - 1));
                    }
                }
                else
                {
                    SimpleValue(item, "tmpSpriteIndex", EditorGUILayout.IntSlider("Sprite Index", SimpleValue<int>(item, "tmpSpriteIndex"), 0, myTarget.tmpSpriteAsset.spriteInfoList.Count - 1));
                }

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                GUILayout.BeginVertical(GUILayout.Width(64), GUILayout.Height(64));
                GUILayout.Label(" ");
                GUILayout.EndVertical();
                DrawTexturePreview(GUILayoutUtility.GetLastRect(), SimpleValue<int>(item, "tmpSpriteIndex"));

                GUILayout.EndHorizontal();
            }
        }

        private void DrawTexturePreview(Rect position, int index)
        {
            if (myTarget.tmpSpriteAsset == null) return;

            try
            {
                Sprite sprite = myTarget.tmpSpriteAsset.spriteGlyphTable[index].sprite;
                Texture texture = sprite.texture;

                Vector2 fullSize = new Vector2(texture.width, texture.height);
                Vector2 size = new Vector2(sprite.textureRect.width, sprite.textureRect.width);

                Rect coords = sprite.textureRect;
                coords.x /= fullSize.x;
                coords.width /= fullSize.x;
                coords.y /= fullSize.y;
                coords.height /= fullSize.y;

                Vector2 ratio;
                ratio.x = position.width / size.x;
                ratio.y = position.height / size.y;
                float minRatio = Mathf.Min(ratio.x, ratio.y);

                Vector2 center = position.center;
                position.width = size.x * minRatio;
                position.height = size.y * minRatio;
                position.center = center;

                GUI.DrawTextureWithTexCoords(position, texture, coords);
            }
            catch { }
        }

        private string SearchBox(string filter)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(4);

            GUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"), GUILayout.Height(20));
            filter = GUILayout.TextField(filter, GetSearchbarStyle());
            GUILayout.Space(-1);
            if (GUILayout.Button(string.Empty, GetSearchbarCancelStyle()))
            {
                filter = string.Empty;
                GUI.FocusControl(null);
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(4);

            GUILayout.EndHorizontal();

            return filter;
        }


        #endregion

    }
}

#endif