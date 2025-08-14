using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    public class GDTKEditor : Editor
    {

        #region Fields

        private static Dictionary<string, Texture2D> icons;
        private static Dictionary<string, GUIStyle> styleStash;
        private static GDTKStyles styles;

        #endregion

        #region Properties

        public static GDTKStyles Styles
        {
            get
            {
                if (styles == null)
                {
                    styles = new GDTKStyles();
                }
                return styles;
            }
        }

        #endregion

        #region Obsolete Public Methods

        public bool SimpleBool(string propertyName)
        {
            return serializedObject.FindProperty(propertyName).boolValue;
        }

        public void SimpleBool(string propertyName, bool value)
        {
            serializedObject.FindProperty(propertyName).boolValue = value;
        }

        public bool SimpleBool(SerializedProperty property, string relativeName)
        {
            return property.FindPropertyRelative(relativeName).boolValue;
        }

        public void SimpleBool(SerializedProperty property, string relativeName, bool value)
        {
            property.FindPropertyRelative(relativeName).boolValue = value;
        }

        public float SimpleFloat(string propertyName)
        {
            return serializedObject.FindProperty(propertyName).floatValue;
        }

        public void SimpleFloat(string propertyName, float value)
        {
            serializedObject.FindProperty(propertyName).floatValue = value;
        }

        public static float SimpleFloat(SerializedProperty property, string relativeName)
        {
            return property.FindPropertyRelative(relativeName).floatValue;
        }

        public static void SimpleFloat(SerializedProperty property, string relativeName, float value)
        {
            property.FindPropertyRelative(relativeName).floatValue = value;
        }

        public int SimpleInt(string propertyName)
        {
            return serializedObject.FindProperty(propertyName).intValue;
        }

        public void SimpleInt(string propertyName, int value)
        {
            serializedObject.FindProperty(propertyName).intValue = value;
        }

        public static int SimpleInt(SerializedProperty property, string relativeName)
        {
            return property.FindPropertyRelative(relativeName).intValue;
        }

        public static void SimpleInt(SerializedProperty property, string relativeName, int value)
        {
            property.FindPropertyRelative(relativeName).intValue = value;
        }

        public string SimpleString(string propertyName)
        {
            return serializedObject.FindProperty(propertyName).stringValue;
        }

        public void SimpleString(string propertyName, string value)
        {
            serializedObject.FindProperty(propertyName).stringValue = value;
        }

        public string SimpleString(SerializedProperty property, string relativeName)
        {
            return property.FindPropertyRelative(relativeName).stringValue;
        }

        public void SimpleString(SerializedProperty property, string relativeName, string value)
        {
            property.FindPropertyRelative(relativeName).stringValue = value;
        }

        #endregion

        #region New UI

        public bool SectionPanel(string title, Texture2D icon, SerializedProperty target, SerializedProperty enabledCheckbox = null)
        {
            bool orgState = target.isExpanded;

            GUILayout.BeginHorizontal(!target.isExpanded ? GDTKEditor.Styles.ButtonPressed : GDTKEditor.Styles.Button, GUILayout.Height(18));

            if (icon != null)
            {
                GUILayout.Label(icon, GUILayout.Height(22), GUILayout.Width(18));
            }

            GUILayout.BeginVertical();
            GUILayout.Space(2);
            GUILayout.BeginHorizontal();

            if (enabledCheckbox != null)
            {
                enabledCheckbox.boolValue = EditorGUILayout.Toggle(enabledCheckbox.boolValue, GUILayout.Width(13));
            }

            GUILayout.Label(title, Styles.SectionHeaderStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseUp)
            {
                target.isExpanded = !target.isExpanded;
                GUI.FocusControl(null);
            }

            return !orgState;
        }

        public static void SimpleSection(SerializedProperty displayProperty, int matchFlag, string title, Texture2D icon, Action drawAction, int spacing = 8)
        {
            int displayFlags = displayProperty.intValue;
            bool hasFlag = (displayFlags & matchFlag) == matchFlag;
            bool toggle = false;
            Color resColor = GUI.contentColor;

            if (spacing != 0) GUILayout.Space(spacing);

            // Container start
            GUILayout.BeginHorizontal();
            //GUILayout.Space(-12);

            // Expand collapse icon
            GUILayout.BeginVertical();
            Color res = GUI.color;
            GUILayout.Space(5);
            Texture2D texture = hasFlag ? Styles.ExpandedIcon : Styles.CollapsedIcon;
            GUI.color = Styles.EditorColor;
            GUILayout.Label(texture, GUILayout.Width(12));
            GUILayout.EndVertical();

            // Icon
            if (icon != null)
            {
                GUILayout.BeginVertical();
                GUILayout.Label(icon, GUILayout.Width(18), GUILayout.Height(18));
                GUILayout.EndVertical();
            }
            GUI.color = res;

            // Title
            GUILayout.BeginVertical();
            if (icon != null)
            {
                GUILayout.Space(2);
            }

            GUILayout.Label(title, Styles.SectionHeaderStyle);
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();

            // Container End
            GUILayout.EndHorizontal();


            // Check Toggle
            if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseUp)
            {
                toggle = true;
            }

            // Draw action
            if (hasFlag)
            {
                drawAction?.Invoke();
            }

            // Perform toggle
            if (toggle)
            {
                GUI.FocusControl(null);
                hasFlag = !hasFlag;
                displayFlags = hasFlag ? displayFlags | matchFlag : displayFlags & ~matchFlag;
                displayProperty.intValue = displayFlags;
            }
        }

        public void SimpleSection(int matchFlag, string title, Texture2D icon, Action drawAction, int spacing = 8, string propertyName = "z_display_flags")
        {
            SimpleSection(serializedObject.FindProperty(propertyName), matchFlag, title, icon, drawAction, spacing);
        }

        #endregion

        #region Public Methods

        public static void ConfirmDelete(SerializedProperty list, int index)
        {
            if (index < 0) return;
            int preSize = list.arraySize;
            list.DeleteArrayElementAtIndex(index);
            if (preSize == list.arraySize)
            {
                list.DeleteArrayElementAtIndex(index);
            }
        }

        public static void DualLabeledSlider(Rect position, SerializedProperty property, GUIContent mainLabel, GUIContent labelLeft, GUIContent labelRight)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            Rect pos = position;

            position.y += 12;
            position.xMin += EditorGUIUtility.labelWidth;
            position.xMax -= EditorGUIUtility.fieldWidth;

            GUI.Label(position, labelLeft, Styles.LabelLeftAligned);
            GUI.Label(position, labelRight, Styles.LabelRightAligned);

            EditorGUI.PropertyField(pos, property, mainLabel);
        }

        public void DrawActionSequenceList(List<ActionSequenceWrapper> actions, UniversalObjectEditorInfo plugins)
        {
            Color resColor = GUI.contentColor;
            GUIStyle style;
            UniversalObjectEditorItemInfo itemInfo;
            int toggleIndex = -1;
            int removeAt = -1;

            float maxWidth = EditorGUIUtility.currentViewWidth - 24 - 40;

            for (int i = 0; i < actions.Count; i++)
            {
                ActionSequenceWrapper wrapper = actions[i];
                itemInfo = plugins.GetInfo(wrapper.instanceId);
                if (itemInfo.editor == null)
                {
                    itemInfo.editor = UniversalPluginEditor.CreateEditor(wrapper.plugin);
                    wrapper.serializationData = SimpleJson.ToJSON(wrapper.plugin);
                }

                // Check drag
                if (plugins.isDragging && plugins.curIndex == i)
                {
                    GUILayout.Space(2);
                    GUILayout.Label(string.Empty, Styles.Redline, GUILayout.Height(1), GUILayout.ExpandWidth(true));
                    GUILayout.Space(2);
                }

                // Draw plugin bar
                GUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(itemInfo.isDragging && itemInfo.isDragging);

                // Item drag handle
                GUI.contentColor = Styles.EditorColor;
                GUILayout.Label(GetIcon("Burger", "icons/burger"), Styles.ButtonLeft, GUILayout.Width(22), GUILayout.Height(22));
                GUI.contentColor = resColor;
                if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDrag)
                {
                    plugins.BeginDrag(i, this, ref itemInfo);
                    GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);
                }

                // Title
                style = new GUIStyle(itemInfo.isExpanded ? Styles.ButtonMidPressed : Styles.ButtonMid);
                GUILayout.BeginHorizontal(style, GUILayout.Height(22), GUILayout.MaxWidth(maxWidth));
                GUILayout.Space(2);
                GUILayout.Label(wrapper.plugin.icon, GUILayout.Height(16), GUILayout.Width(16));
                GUILayout.Space(2);
                GUILayout.Label(wrapper.plugin.titlebarText, Styles.TitleTextStyle, GUILayout.Height(15));
                GUILayout.EndHorizontal();
                if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
                {
                    GUI.FocusControl(null);
                    toggleIndex = i;
                }

                // Delete
                GUI.contentColor = Styles.EditorColor;
                GUILayout.Label(GetIcon("Trash", "icons/trash"), Styles.ButtonRight, GUILayout.Width(22), GUILayout.Height(22));
                GUI.contentColor = resColor;
                if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
                {
                    GUI.FocusControl(null);
                    removeAt = i;
                }

                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();

                if (itemInfo.isExpanded)
                {
                    GUILayout.Space(-5);
                    GUILayout.BeginVertical("box");
                    itemInfo.editor.OnInspectorGUI();
                    GUILayout.EndVertical();
                }

                if (itemInfo.editor.IsDirty)
                {
                    itemInfo.editor.ApplyUpdates(ref wrapper.serializationData);
                    EditorUtility.SetDirty(target);
                }
            }

            if (toggleIndex > -1)
            {
                plugins.items[actions[toggleIndex].instanceId].isExpanded = !plugins.items[actions[toggleIndex].instanceId].isExpanded;
                if (plugins.items[actions[toggleIndex].instanceId].isExpanded)
                {
                    plugins.items[actions[toggleIndex].instanceId].editor.OnEnable();
                }
            }

            if (removeAt > -1)
            {
                plugins.items.Remove(actions[removeAt].instanceId);
                actions.RemoveAt(removeAt);
                EditorUtility.SetDirty(target);
            }
        }

        public void DrawFormattedLabelList(SerializedProperty list, bool showAdd = true)
        {
            int removeAt = -1;
            SerializedProperty entry;

            SectionHeader("Formatted Labels", GetIcon("icons/list"));
            for (int i = 0; i < list.arraySize; i++)
            {
                GUILayout.BeginVertical("box");

                entry = list.GetArrayElementAtIndex(i);
                SimpleProperty(entry, "target");
                SimpleProperty(entry, "format");
                if (GUILayout.Button("Remove"))
                {
                    GUI.FocusControl(null);
                    removeAt = i;
                }

                GUILayout.EndVertical();
            }

            if (removeAt >= 0)
            {
                ConfirmDelete(list, removeAt);
            }

            GUILayout.BeginHorizontal();
            GUILayout.Space(8);

            GUILayout.FlexibleSpace();
            if (showAdd)
            {
                if (GUILayout.Button("Add"))
                {
                    list.arraySize++;
                }
            }
            EditorGUI.BeginDisabledGroup(list.arraySize == 0);
            if (GUILayout.Button("Clear")) { list.arraySize = 0; }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();

        }

        public static bool DrawBarIcon(GUIStyle style, Texture icon, string tooltip = "")
        {
            Color resColor = GUI.contentColor;

            GUILayout.BeginHorizontal(style, GUILayout.Height(22), GUILayout.Width(22));
            GUILayout.BeginVertical();
            GUILayout.Space(1);
            GUI.contentColor = Styles.EditorColor;
            GUILayout.Label(new GUIContent(icon, tooltip), GUILayout.Height(16), GUILayout.Width(16));
            GUI.contentColor = resColor;
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
            {
                GUI.FocusControl(null);
                return true;
            }

            return false;
        }

        public static bool DrawToggleBarTitle(bool value, string text)
        {
            GUIStyle style = new GUIStyle(value ? Styles.ButtonPressed : Styles.Button);
            GUILayout.BeginHorizontal(style, GUILayout.Height(22));
            GUILayout.BeginVertical();
            GUILayout.Space(1);
            GUILayout.Label(text, Styles.TitleTextStyle, GUILayout.Height(15));
            GUILayout.EndVertical();
            //GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
            {
                GUI.FocusControl(null);
                return !value;
            }
            return value;
        }

        public static bool DrawToggleBarTitleMid(bool value, string text)
        {
            GUIStyle style = new GUIStyle(value ? Styles.ButtonMidPressed : Styles.ButtonMid);
            GUILayout.BeginHorizontal(style, GUILayout.Height(22));
            GUILayout.BeginVertical();
            GUILayout.Space(1);
            GUILayout.Label(text, Styles.TitleTextStyle, GUILayout.Height(15));
            GUILayout.EndVertical();
            //GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
            {
                GUI.FocusControl(null);
                return !value;
            }
            return value;
        }

        public static bool DrawToggleBarTitleLeft(bool value, string text)
        {
            GUIStyle style = new GUIStyle(value ? Styles.ButtonLeftPressed : Styles.ButtonLeft);
            GUILayout.BeginHorizontal(style, GUILayout.Height(22));
            GUILayout.BeginVertical();
            GUILayout.Space(1);
            GUILayout.Label(text, Styles.TitleTextStyle, GUILayout.Height(15));
            GUILayout.EndVertical();
            //GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
            {
                GUI.FocusControl(null);
                return !value;
            }
            return value;
        }

        public Tuple<int, bool> ExpandableListItem(string displayName, bool expanded, bool showDelete = true)
        {
            int expandChange = 0;
            bool deleteItem = false;
            Color resColor = GUI.contentColor;

            // Draw bar
            GUILayout.BeginHorizontal();

            // Title
            GUILayout.BeginHorizontal(showDelete ? expanded ? Styles.ButtonLeftPressed : Styles.ButtonLeft : expanded ? Styles.ButtonPressed : Styles.Button, GUILayout.Height(22));
            GUILayout.BeginVertical();
            GUILayout.Space(1);
            GUILayout.Label(displayName, Styles.TitleTextStyle, GUILayout.Height(15));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
            {
                expandChange = expanded ? -1 : 1;
                GUI.FocusControl(null);
            }

            // Delete button
            if (showDelete)
            {
                GUI.contentColor = Styles.EditorColor;
                GUILayout.Label(GetIcon("Trash", "Icons/trash"), Styles.ButtonRight, GUILayout.Width(22), GUILayout.Height(22));
                GUI.contentColor = resColor;
                if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
                {
                    GUI.FocusControl(null);
                    deleteItem = true;
                }
            }

            GUILayout.EndHorizontal();

            return new Tuple<int, bool>(expandChange, deleteItem);
        }

        public static List<T> FindAssetsOfType<T>() where T : ScriptableObject
        {
            return AssetDatabase.FindAssets($"t: {typeof(T).Name}").ToList().Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<T>).ToList();
        }

        public static Texture2D GetIcon(string path)
        {
            return GetIcon(path, path);
        }

        public static Texture2D GetIcon(string name, string path)
        {
            if (icons == null) icons = new Dictionary<string, Texture2D>();

            if (icons.ContainsKey(name))
            {
                return icons[name];
            }

            icons.Add(name, (Texture2D)Resources.Load(path, typeof(Texture2D)));
            return icons[name];
        }

        public static GUIStyle GetSearchbarStyle()
        {
            if (styleStash == null) styleStash = new Dictionary<string, GUIStyle>();
            if (styleStash.ContainsKey("searchbar")) return styleStash["searchbar"];

            GUIStyle result = GUI.skin.FindStyle("ToolbarSeachTextField");
            if (result != null)
            {
                styleStash.Add("searchbar", result);
                return result;
            }

            result = GUI.skin.FindStyle("ToolbarSearchTextField");
            styleStash.Add("searchbar", result);
            return result;

        }

        public static GUIStyle GetSearchbarCancelStyle()
        {
            if (styleStash == null) styleStash = new Dictionary<string, GUIStyle>();
            if (styleStash.ContainsKey("searchbarCancel")) return styleStash["searchbarCancel"];

            GUIStyle result = GUI.skin.FindStyle("ToolbarSeachCancelButton");
            if (result != null)
            {
                styleStash.Add("searchbarCancel", result);
                return result;
            }

            result = GUI.skin.FindStyle("ToolbarSearchCancelButton");
            styleStash.Add("searchbarCancel", result);
            return result;

        }

        public void MainContainerBegin()
        {
            serializedObject.Update();
            GUILayout.BeginVertical();
        }

        public void MainContainerEnd(bool showCopyright = true)
        {
            if (showCopyright)
            {
                GUILayout.Space(8);

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("© NULLSAVE", Styles.FooterStyle);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }

        public void RemoveFromList(SerializedProperty list, int index)
        {
            if (index < 0) return;
            int preSize = list.arraySize;
            list.DeleteArrayElementAtIndex(index);
            if (preSize == list.arraySize)
            {
                list.DeleteArrayElementAtIndex(index);
            }
        }

        public static void SearchableList(string title, string[] options, int index, Action<int> setValue)
        {
            if (options.Length < 20)
            {
                setValue?.Invoke(EditorGUILayout.Popup(title, index, options));
                return;
            }

            Rect r = EditorGUILayout.GetControlRect(true, GUILayout.ExpandWidth(true));
            GUILayout.Space(-EditorGUIUtility.singleLineHeight);
            GUILayout.BeginHorizontal();

            GUILayout.Label(title, GUILayout.Width(EditorGUIUtility.labelWidth - 4));
            if (GUILayout.Button(index < 0 || index > options.Length - 1 ? string.Empty : options[index], Styles.Button))
            {
                GUI.FocusControl(null);
                PopupWindow.Show(r, new PopupSearchWindow() { width = r.width, objs = options.ToList(), startIndex = index, onSelection = setValue });
            }
            GUILayout.EndHorizontal();

        }

        public bool SectionGroup(string title, Texture2D icon, bool expand, string listName = null, Type acceptedType = null, bool preventDupliates = true)
        {
            bool resValue = expand;
            SerializedProperty list = serializedObject.FindProperty(listName);
            bool displayList = list != null && acceptedType != null;

            // Top spacing
            GUILayout.Space(8);

            // Container start
            GUILayout.BeginHorizontal();
            //GUILayout.Space(-12);

            // Expand collapse icon
            GUILayout.BeginVertical();
            Color res = GUI.color;
            if (displayList)
            {
                GUILayout.Space(7);
            }
            else
            {
                GUILayout.Space(5);
            }
            Texture2D texture = resValue ? Styles.ExpandedIcon : Styles.CollapsedIcon;
            GUI.color = Styles.EditorColor;
            GUILayout.Label(texture, GUILayout.Width(12));
            GUILayout.EndVertical();

            // Icon
            if (icon != null)
            {
                GUILayout.BeginVertical();
                if (displayList)
                {
                    GUILayout.Space(4);

                }
                GUILayout.Label(icon, GUILayout.Width(18), GUILayout.Height(18));
                GUILayout.EndVertical();
            }
            GUI.color = res;

            // Title
            GUILayout.BeginVertical();
            if (icon != null)
            {
                if (displayList)
                {
                    GUILayout.Space(4);
                }
                else
                {
                    GUILayout.Space(2);
                }
            }

            GUILayout.Label(title, Styles.SectionHeaderStyle);
            GUILayout.EndVertical();

            // Drag and drop
            if (displayList)
            {
                DrawDragDropIcon(res);
            }

            GUILayout.FlexibleSpace();

            // Container End
            GUILayout.EndHorizontal();

            if (displayList)
            {
                if (ProcessDragDrop(list, acceptedType, preventDupliates)) resValue = true;
            }

            // Toggle
            if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseUp)
            {
                GUI.FocusControl(null);
                resValue = !resValue;
                Repaint();
            }

            GUILayout.Space(4);

            return resValue;
        }

        public bool SectionGroup(string buttonText, out bool buttonPressed, string title, Texture2D icon, bool expand, string listName = null, Type acceptedType = null, bool preventDuplicates = true)
        {
            bool resValue = expand;
            SerializedProperty list = serializedObject.FindProperty(listName);
            bool displayList = list != null && acceptedType != null;

            // Top spacing
            GUILayout.Space(8);

            // Container start
            GUILayout.BeginHorizontal();

            // Expand collapse icon
            GUILayout.BeginVertical();
            Color res = GUI.color;
            if (displayList)
            {
                GUILayout.Space(7);
            }
            else
            {
                GUILayout.Space(5);
            }
            Texture2D texture = resValue ? Styles.ExpandedIcon : Styles.CollapsedIcon;
            GUI.color = Styles.EditorColor;
            GUILayout.Label(texture, GUILayout.Width(12));
            GUILayout.EndVertical();

            // Icon
            if (icon != null)
            {
                GUILayout.BeginVertical();
                if (displayList)
                {
                    GUILayout.Space(4);

                }
                GUILayout.Label(icon, GUILayout.Width(18), GUILayout.Height(18));
                GUILayout.EndVertical();
            }
            GUI.color = res;

            // Title
            GUILayout.BeginVertical();
            if (icon != null)
            {
                if (displayList)
                {
                    GUILayout.Space(4);
                }
                else
                {
                    GUILayout.Space(2);
                }
            }

            GUILayout.Label(title, Styles.SectionHeaderStyle);
            GUILayout.EndVertical();

            // Drag and drop
            if (displayList)
            {
                DrawDragDropIcon(res);
            }


            GUILayout.FlexibleSpace();

            buttonPressed = GUILayout.Button(buttonText);

            // Container End
            GUILayout.EndHorizontal();

            // Drag and drop
            if (displayList)
            {
                if (ProcessDragDrop(list, acceptedType, preventDuplicates)) resValue = true;
            }


            // Toggle
            if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseUp)
            {
                GUI.FocusControl(null);
                resValue = !resValue;
                Repaint();
            }

            GUILayout.Space(4);

            return resValue;
        }

        public void SectionHeader(string title, string listName = null, Type acceptedType = null, bool preventDuplicates = true)
        {
            SerializedProperty list = serializedObject.FindProperty(listName);
            bool displayList = list != null && acceptedType != null;

            GUILayout.Space(displayList ? 8 : 12);
            GUILayout.BeginHorizontal();
            GUILayout.Space(0);

            if (!displayList)
            {
                GUILayout.Label(title, Styles.SectionHeaderStyle);
            }
            else
            {
                GUILayout.BeginVertical();
                GUILayout.Label(title, Styles.SectionHeaderStyle);
                GUILayout.EndVertical();

                // Drag and drop
                if (displayList)
                {
                    Color res = GUI.color;
                    GUILayout.BeginVertical();
                    GUI.color = Styles.EditorColor;
                    GUILayout.Space(4);
                    GUILayout.Label(GetIcon("Drop", "Icons/drop"), GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false), GUILayout.Height(16), GUILayout.Width(18));
                    GUI.color = res;
                    GUILayout.EndVertical();
                    GUILayout.FlexibleSpace();
                }

            }

            GUILayout.EndHorizontal();

            if (displayList)
            {
                ProcessDragDrop(list, acceptedType, preventDuplicates);
            }

            GUILayout.Space(4);
        }

        public void SectionHeader(string title, Texture2D icon)
        {
            // Top spacing
            //GUILayout.Space(8);

            // Container start
            GUILayout.BeginHorizontal();

            // Icon
            if (icon != null)
            {
                Color res = GUI.color;
                GUILayout.BeginVertical();
                GUILayout.Label(icon, GUILayout.Width(18), GUILayout.Height(18));
                GUILayout.EndVertical();
                GUI.color = res;
            }

            // Title
            GUILayout.BeginVertical();
            if (icon != null)
            {
                GUILayout.Space(2);
            }

            GUILayout.Label(title, Styles.SectionHeaderStyle);
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();

            // Container End
            GUILayout.EndHorizontal();
            GUILayout.Space(4);
        }

        public void SectionHeaderWithButton(string title, string buttonText, out bool buttonPressed, string listName = null, Type acceptedType = null, bool preventDuplicates = true)
        {
            SerializedProperty list = serializedObject.FindProperty(listName);
            bool displayList = list != null && acceptedType != null;

            GUILayout.Space(displayList ? 8 : 12);
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);

            if (!displayList)
            {
                GUILayout.Label(title, Styles.SectionHeaderStyle);
            }
            else
            {
                GUILayout.BeginVertical();
                GUILayout.Space(5);
                GUILayout.Label(title, Styles.SectionHeaderStyle);
                GUILayout.EndVertical();

                // Drag and drop
                if (displayList)
                {
                    Color res = GUI.color;
                    GUILayout.BeginVertical();
                    GUI.color = Styles.EditorColor;
                    GUILayout.Space(4);
                    GUILayout.Label(GetIcon("Drop", "Icons/drop"), GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false), GUILayout.Height(16), GUILayout.Width(18));
                    GUI.color = res;
                    GUILayout.EndVertical();
                    GUILayout.FlexibleSpace();
                }
            }

            GUILayout.FlexibleSpace();
            buttonPressed = GUILayout.Button(buttonText);

            GUILayout.EndHorizontal();

            if (displayList)
            {
                ProcessDragDrop(list, acceptedType, preventDuplicates);
            }

            GUILayout.Space(4);
        }

        public bool SectionToggle(bool expanded, string title, Texture2D icon = null)
        {
            bool result = expanded;
            Color res = GUI.color;

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Space(5);
            Texture2D texture = result ? Styles.ExpandedIcon : Styles.CollapsedIcon;
            GUI.color = Styles.EditorColor;
            GUILayout.Label(texture, GUILayout.Width(10));
            GUILayout.EndVertical();

            if (icon != null)
            {
                GUI.color = Styles.EditorColor;
                GUILayout.BeginVertical();
                GUILayout.Label(icon, GUILayout.Width(18), GUILayout.Height(18));
                GUILayout.EndVertical();
                GUI.color = res;
            }

            GUILayout.Label(title, Styles.SectionHeaderStyle);
            GUILayout.FlexibleSpace();


            GUILayout.EndHorizontal();

            // Toggle
            if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseUp)
            {
                GUI.FocusControl(null);
                result = !result;
            }

            return result;
        }

        public bool SectionToggle(int displayFlags, int flag, string title, Texture2D icon = null, string listName = null, Type acceptedType = null, bool preventDuplicates = true, string propertyName = "z_display_flags")
        {
            bool hasFlag = (displayFlags & flag) == flag;
            bool result = SectionGroup(title, icon, hasFlag, listName, acceptedType, preventDuplicates);

            if (result != hasFlag)
            {
                displayFlags = result ? displayFlags | flag : displayFlags & ~flag;
                serializedObject.FindProperty(propertyName).intValue = displayFlags;
            }

            return hasFlag;
        }

        public bool SectionToggle(int displayFlags, int flag, string buttonText, out bool buttonPressed, string title, Texture2D icon = null, string listName = null, Type acceptedType = null, bool preventDuplicates = true, string propertyName = "z_display_flags")
        {
            bool hasFlag = (displayFlags & flag) == flag;
            bool result = SectionGroup(buttonText, out buttonPressed, title, icon, hasFlag, listName, acceptedType, preventDuplicates);

            if (result != hasFlag)
            {
                displayFlags = result ? displayFlags | flag : displayFlags & ~flag;
                serializedObject.FindProperty(propertyName).intValue = displayFlags;
            }

            return hasFlag;
        }

        public void SimpleEnumFlagsContext(SerializedProperty property, Type enumType)
        {
            SimpleEnumFlagsContext(property, ObjectNames.NicifyVariableName(property.name), enumType);
        }

        public void SimpleEnumFlagsContext(SerializedProperty property, string title, Type enumType)
        {
            Rect r = EditorGUILayout.GetControlRect(true, GUILayout.ExpandWidth(true));
            GUILayout.Space(-EditorGUIUtility.singleLineHeight);

            GUILayout.BeginHorizontal();
            GUILayout.Label(title, EditorStyles.label, GUILayout.Width(EditorGUIUtility.labelWidth));

            Enum ev = (Enum)Enum.ToObject(enumType, property.intValue);
            string enumTitle;
            if (property.intValue == 0)
            {
                enumTitle = "(None)";
            }
            else
            {
                enumTitle = ev.ToString();
            }

            if (GUILayout.Button(enumTitle, EditorStyles.popup))
            {
                GUI.FocusControl(null);
                PopupWindow.Show(r, new EnumFlagsPopupWindow()
                {
                    width = r.width,
                    value = ev,
                    onSelection = i =>
                    {
                        property.serializedObject.Update();
                        property.intValue = i;
                        property.serializedObject.ApplyModifiedProperties();
                    }
                });
            }
            GUILayout.EndHorizontal();
        }

        public void SimpleList(string listName, bool showAdd = true)
        {
            SimpleList(serializedObject.FindProperty(listName), showAdd);
        }

        public void SimpleList(SerializedProperty list, bool showAdd = true)
        {
            for (int i = 0; i < list.arraySize; i++)
            {
                if (i < list.arraySize && i >= 0)
                {
                    EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), new GUIContent(string.Empty, null, string.Empty));
                }
            }

            GUILayout.BeginHorizontal();
            GUILayout.Space(8);
            GUILayout.BeginVertical();
            if (list.arraySize > 0)
            {
                GUILayout.Space(4);
                GUILayout.Label("Right-click item to remove", Styles.FooterStyle);
            }
            else
            {
                GUILayout.Label("{Empty}", Styles.FooterStyle);
            }
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            if (showAdd)
            {
                if (GUILayout.Button("Add"))
                {
                    list.arraySize++;
                }
            }
            EditorGUI.BeginDisabledGroup(list.arraySize == 0);
            if (GUILayout.Button("Clear")) { list.arraySize = 0; }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();
        }

        public void SimpleProperty(string propertyName)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(propertyName));
        }

        public void SimpleProperty(string propertyName, string title)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(propertyName), new GUIContent(title, null, string.Empty));
        }

        public void SimpleProperty(SerializedProperty property, string relativeName)
        {
            EditorGUILayout.PropertyField(property.FindPropertyRelative(relativeName));
        }

        public void SimpleProperty(SerializedProperty property, string relativeName, string title, string tooltip = null)
        {
            EditorGUILayout.PropertyField(property.FindPropertyRelative(relativeName), new GUIContent(title, null, tooltip));
        }

        public int SimpleObjectDragList(SerializedProperty list, string titlePropertyName, EditorInfoList infoList, Action<SerializedProperty, int> drawItemAction, bool showDuplicate = false)
        {
            int delAt = -1;
            int dupId = -1;
            EditorInfoItem itemInfo;
            Color resColor = GUI.contentColor;
            bool expandAtEnd;

            for (int i = 0; i < list.arraySize; i++)
            {
                expandAtEnd = false;

                SerializedProperty item = list.GetArrayElementAtIndex(i);
                itemInfo = infoList.GetInfo("item" + i);

                // Check drag
                if (infoList.isDragging && i == infoList.curIndex && i <= infoList.startIndex)
                {
                    GUILayout.Space(2);
                    GUILayout.Label(string.Empty, Styles.Redline, GUILayout.Height(1), GUILayout.ExpandWidth(true));
                    GUILayout.Space(2);
                }

                // Draw bar
                GUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(itemInfo.isDragging && itemInfo.isDragging);

                // Item drag handle
                GUI.contentColor = Styles.EditorColor;
                GUILayout.Label(GetIcon("Burger", "Icons/burger"), Styles.ButtonLeft, GUILayout.Width(22), GUILayout.Height(22));
                GUI.contentColor = resColor;
                if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDrag)
                {
                    infoList.BeginDrag(i, this, ref itemInfo);
                    GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);
                }

                // Title
                GUILayout.BeginHorizontal(itemInfo.isExpanded ? Styles.ButtonMidPressed : Styles.ButtonMid, GUILayout.Height(22));
                GUILayout.BeginVertical();
                GUILayout.Space(1);
                GUILayout.Label(item.FindPropertyRelative(titlePropertyName).stringValue, Styles.TitleTextStyle, GUILayout.Height(15));
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();

                itemInfo.rect = GUILayoutUtility.GetLastRect();
                if (itemInfo.rect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
                {
                    expandAtEnd = true;
                }

                if (showDuplicate)
                {
                    GUI.contentColor = Styles.EditorColor;
                    GUILayout.Label(GetIcon("Icons/duplicate"), Styles.ButtonMid, GUILayout.Width(22), GUILayout.Height(22));
                    GUI.contentColor = resColor;
                    if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
                    {
                        GUI.FocusControl(null);
                        dupId = i;
                    }
                }

                // Delete
                GUI.contentColor = Styles.EditorColor;
                GUILayout.Label(GetIcon("Icons/trash"), Styles.ButtonRight, GUILayout.Width(22), GUILayout.Height(22));
                GUI.contentColor = resColor;
                if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
                {
                    GUI.FocusControl(null);
                    delAt = i;
                }

                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();

                if (itemInfo.isExpanded)
                {
                    GUILayout.Space(-5);
                    GUILayout.BeginVertical("box");
                    if (drawItemAction != null)
                    {
                        drawItemAction.Invoke(item, i);
                    }
                    GUILayout.EndVertical();
                    itemInfo.rect.height += GUILayoutUtility.GetLastRect().height;
                }

                if (expandAtEnd)
                {
                    itemInfo.isExpanded = !itemInfo.isExpanded;
                    infoList.items["item" + i] = itemInfo;
                    Repaint();
                }
                else
                {
                    infoList.items["item" + i] = itemInfo;
                }

                if (infoList.isDragging && i == infoList.curIndex && i > infoList.startIndex)
                {
                    GUILayout.Space(2);
                    GUILayout.Label(string.Empty, Styles.Redline, GUILayout.Height(1), GUILayout.ExpandWidth(true));
                    GUILayout.Space(2);
                }
            }

            infoList.UpdateDragPosition(list, this, true);

            if (dupId >= 0)
            {
                list.InsertArrayElementAtIndex(dupId);
            }

            return delAt;
        }

        public void SimpleObjectDragList(SerializedProperty list, Action<SerializedProperty, int> drawItemTitleAction, EditorInfoList infoList, Action<SerializedProperty, int> drawItemAction, bool showDuplicate = false)
        {
            int delAt = -1;
            int dupId = -1;
            EditorInfoItem itemInfo;
            Color resColor = GUI.contentColor;
            bool expandAtEnd;

            for (int i = 0; i < list.arraySize; i++)
            {
                expandAtEnd = false;

                SerializedProperty item = list.GetArrayElementAtIndex(i);
                itemInfo = infoList.GetInfo("item" + i);

                // Check drag
                if (infoList.isDragging && i == infoList.curIndex && i <= infoList.startIndex)
                {
                    GUILayout.Space(2);
                    GUILayout.Label(string.Empty, Styles.Redline, GUILayout.Height(1), GUILayout.ExpandWidth(true));
                    GUILayout.Space(2);
                }

                // Draw bar
                GUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(itemInfo.isDragging && itemInfo.isDragging);

                // Item drag handle
                GUI.contentColor = Styles.EditorColor;
                GUILayout.Label(GetIcon("Burger", "Icons/burger"), Styles.ButtonLeft, GUILayout.Width(22), GUILayout.Height(22));
                GUI.contentColor = resColor;
                if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDrag)
                {
                    infoList.BeginDrag(i, this, ref itemInfo);
                    GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);
                }

                // Title
                GUILayout.BeginHorizontal(itemInfo.isExpanded ? Styles.ButtonMidPressed : Styles.ButtonMid, GUILayout.Height(22));
                GUILayout.BeginVertical();
                drawItemTitleAction.Invoke(item, i);
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();

                itemInfo.rect = GUILayoutUtility.GetLastRect();
                if (itemInfo.rect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
                {
                    expandAtEnd = true;
                }

                if (showDuplicate)
                {
                    GUI.contentColor = Styles.EditorColor;
                    GUILayout.Label(GetIcon("Icons/duplicate"), Styles.ButtonMid, GUILayout.Width(22), GUILayout.Height(22));
                    GUI.contentColor = resColor;
                    if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
                    {
                        GUI.FocusControl(null);
                        dupId = i;
                    }
                }

                // Delete
                GUI.contentColor = Styles.EditorColor;
                GUILayout.Label(GetIcon("Trash", "Icons/trash"), Styles.ButtonRight, GUILayout.Width(22), GUILayout.Height(22));
                GUI.contentColor = resColor;
                if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
                {
                    GUI.FocusControl(null);
                    delAt = i;
                }

                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();

                if (itemInfo.isExpanded)
                {
                    GUILayout.Space(-5);
                    GUILayout.BeginVertical("box");
                    if (drawItemAction != null)
                    {
                        drawItemAction.Invoke(item, i);
                    }
                    GUILayout.EndVertical();
                    itemInfo.rect.height += GUILayoutUtility.GetLastRect().height;
                }

                if (expandAtEnd)
                {
                    itemInfo.isExpanded = !itemInfo.isExpanded;
                    infoList.items["item" + i] = itemInfo;
                    Repaint();
                }
                else
                {
                    infoList.items["item" + i] = itemInfo;
                }

                if (infoList.isDragging && i == infoList.curIndex && i > infoList.startIndex)
                {
                    GUILayout.Space(2);
                    GUILayout.Label(string.Empty, Styles.Redline, GUILayout.Height(1), GUILayout.ExpandWidth(true));
                    GUILayout.Space(2);
                }
            }

            infoList.UpdateDragPosition(list, this, true);

            if (dupId >= 0)
            {
                list.InsertArrayElementAtIndex(dupId);
            }

            if (delAt >= 0)
            {
                ConfirmDelete(list, delAt);
            }

        }

        public int SimpleSODragList(SerializedProperty list, EditorInfoList infoList, Action<Editor> editorDrawOverride = null)
        {
            int delAt = -1;
            EditorInfoItem itemInfo;
            Color resColor = GUI.contentColor;
            bool expandAtEnd, selectAtEnd;

            for (int i = 0; i < list.arraySize; i++)
            {
                expandAtEnd = false;
                selectAtEnd = false;

                UnityEngine.Object item = list.GetArrayElementAtIndex(i).objectReferenceValue;
                if (item == null)
                {
                    continue;
                }
                itemInfo = infoList.GetInfo(item.name);
                if (itemInfo.editor == null)
                {
                    itemInfo.editor = CreateEditor(item);
                }

                // Check drag
                if (infoList.isDragging && i == infoList.curIndex && i <= infoList.startIndex)
                {
                    GUILayout.Space(2);
                    GUILayout.Label(string.Empty, Styles.Redline, GUILayout.Height(1), GUILayout.ExpandWidth(true));
                    GUILayout.Space(2);
                }

                // Draw bar
                GUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(itemInfo.isDragging && itemInfo.isDragging);

                // Item drag handle
                GUI.contentColor = Styles.EditorColor;
                GUILayout.Label(GetIcon("Burger", "Icons/burger"), Styles.ButtonLeft, GUILayout.Width(22), GUILayout.Height(22));
                GUI.contentColor = resColor;
                if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDrag)
                {
                    infoList.BeginDrag(i, this, ref itemInfo);
                    GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);
                }

                // Title
                GUILayout.BeginHorizontal(itemInfo.isExpanded ? Styles.ButtonMidPressed : Styles.ButtonMid, GUILayout.Height(22));
                GUILayout.BeginVertical();
                GUILayout.Space(1);
                GUILayout.Label(item.name, Styles.TitleTextStyle, GUILayout.Height(15));
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                itemInfo.rect = GUILayoutUtility.GetLastRect();
                if (itemInfo.rect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
                {
                    expandAtEnd = true;
                }

                // Select Object
                GUI.contentColor = Styles.EditorColor;
                GUILayout.Label(GetIcon("View", "Icons/view"), Styles.ButtonMid, GUILayout.Width(22), GUILayout.Height(22));
                GUI.contentColor = resColor;
                if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
                {
                    Event.current.Use();
                    selectAtEnd = true;
                }

                // Delete
                GUI.contentColor = Styles.EditorColor;
                GUILayout.Label(GetIcon("Trash", "Icons/trash"), Styles.ButtonRight, GUILayout.Width(22), GUILayout.Height(22));
                GUI.contentColor = resColor;
                if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
                {
                    GUI.FocusControl(null);
                    delAt = i;
                }

                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();

                if (itemInfo.isExpanded)
                {
                    GUILayout.Space(-5);
                    GUILayout.BeginVertical("box");
                    itemInfo.editor.serializedObject.Update();
                    if (editorDrawOverride == null)
                    {
                        itemInfo.editor.OnInspectorGUI();
                    }
                    else
                    {
                        editorDrawOverride.Invoke(itemInfo.editor);
                    }
                    itemInfo.editor.serializedObject.ApplyModifiedProperties();
                    GUILayout.EndVertical();
                    itemInfo.rect.height += GUILayoutUtility.GetLastRect().height;
                }

                if (expandAtEnd)
                {
                    itemInfo.isExpanded = !itemInfo.isExpanded;
                    infoList.items[item.name] = itemInfo;
                    Repaint();
                }
                else
                {
                    infoList.items[item.name] = itemInfo;
                }

                if (infoList.isDragging && i == infoList.curIndex && i > infoList.startIndex)
                {
                    GUILayout.Space(2);
                    GUILayout.Label(string.Empty, Styles.Redline, GUILayout.Height(1), GUILayout.ExpandWidth(true));
                    GUILayout.Space(2);
                }

                if (selectAtEnd)
                {
                    Selection.activeObject = item;
                    return delAt;
                }
            }

            infoList.UpdateDragPosition(list, this, false);
            return delAt;
        }

        public void SimpleStringSearchListProperty(SerializedProperty list, string title, List<string> options)
        {
            Rect r = EditorGUILayout.GetControlRect(true, GUILayout.ExpandWidth(true));
            GUILayout.Space(-EditorGUIUtility.singleLineHeight);

            GUILayout.BeginHorizontal();
            GUILayout.Label(title, EditorStyles.label, GUILayout.Width(EditorGUIUtility.labelWidth));
            GUILayout.Label("(" + list.arraySize + ")");
            GUILayout.EndHorizontal();

            SerializedProperty entry;
            int removeAt = -1;
            for (int i = 0; i < list.arraySize; i++)
            {
                entry = list.GetArrayElementAtIndex(i);
                GUILayout.BeginHorizontal();
                GUILayout.Space(EditorGUIUtility.labelWidth + 4);
                GUILayout.BeginVertical(Styles.SlimBox);
                GUILayout.Label(entry.stringValue);
                GUILayout.EndVertical();
                if (GUILayout.Button(GetIcon("icons/trash-small"), GUILayout.Width(32), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    GUI.FocusControl(null);
                    removeAt = i;
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            GUILayout.Space(EditorGUIUtility.labelWidth + 4);
            if (GUILayout.Button(new GUIContent("Add New", GetIcon("icons/add-small")), GUILayout.Height(24)))
            {
                GUI.FocusControl(null);
                PopupWindow.Show(r, new SearchabelStringPopupWindow()
                {
                    width = r.width,
                    objs = options,
                    onSelection = s =>
                    {
                        list.serializedObject.Update();
                        list.arraySize++;
                        list.GetArrayElementAtIndex(list.arraySize - 1).stringValue = s;
                        list.serializedObject.ApplyModifiedProperties();
                    }
                });

            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            RemoveFromList(list, removeAt);
        }

        public void SimpleStringSearchListProperty(SerializedProperty list, string title, Action<List<string>> buildOptions)
        {
            Rect r = EditorGUILayout.GetControlRect(true, GUILayout.ExpandWidth(true));
            GUILayout.Space(-EditorGUIUtility.singleLineHeight);

            GUILayout.BeginHorizontal();
            GUILayout.Label(title, EditorStyles.label, GUILayout.Width(EditorGUIUtility.labelWidth));
            GUILayout.Label("(" + list.arraySize + ")");
            GUILayout.EndHorizontal();

            SerializedProperty entry;
            int removeAt = -1;
            for (int i = 0; i < list.arraySize; i++)
            {
                entry = list.GetArrayElementAtIndex(i);
                GUILayout.BeginHorizontal();
                GUILayout.Space(EditorGUIUtility.labelWidth + 4);
                GUILayout.BeginVertical(Styles.SlimBox);
                GUILayout.Label(entry.stringValue);
                GUILayout.EndVertical();
                if (GUILayout.Button(GetIcon("icons/trash-small"), GUILayout.Width(32), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    GUI.FocusControl(null);
                    removeAt = i;
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            GUILayout.Space(EditorGUIUtility.labelWidth + 4);
            if (GUILayout.Button(new GUIContent("Add New", GetIcon("icons/add-small")), GUILayout.Height(24)))
            {
                GUI.FocusControl(null);
                PopupWindow.Show(r, new SearchabelStringPopupWindow()
                {
                    width = r.width,
                    buildOptions = buildOptions,
                    onSelection = s =>
                    {
                        list.serializedObject.Update();
                        list.arraySize++;
                        list.GetArrayElementAtIndex(list.arraySize - 1).stringValue = s;
                        list.serializedObject.ApplyModifiedProperties();
                    }
                });

            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            RemoveFromList(list, removeAt);
        }

        public void SimpleStringSearchHeaderListProperty(SerializedProperty list, string title, Texture2D icon, Action<List<string>> buildOptions)
        {
            Rect r = EditorGUILayout.GetControlRect(true, GUILayout.ExpandWidth(true));
            GUILayout.Space(-EditorGUIUtility.singleLineHeight);

            GUILayout.BeginHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Width(EditorGUIUtility.labelWidth - 4));
            if (icon != null)
            {
                Color res = GUI.color;
                GUILayout.BeginVertical();
                GUILayout.Label(icon, GUILayout.Width(18), GUILayout.Height(18));
                GUILayout.EndVertical();
                GUI.color = res;
            }

            GUILayout.BeginVertical();
            if (icon != null)
            {
                GUILayout.Space(2);
            }

            GUILayout.Label(title, Styles.SectionHeaderStyle);
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();

            GUILayout.Label("(" + list.arraySize + ")");

            GUILayout.EndHorizontal();

            SerializedProperty entry;
            int removeAt = -1;
            for (int i = 0; i < list.arraySize; i++)
            {
                entry = list.GetArrayElementAtIndex(i);
                GUILayout.BeginHorizontal();
                GUILayout.Space(EditorGUIUtility.labelWidth + 4);
                GUILayout.BeginVertical(Styles.SlimBox);
                GUILayout.Label(entry.stringValue);
                GUILayout.EndVertical();
                if (GUILayout.Button(GetIcon("icons/trash-small"), GUILayout.Width(32), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    GUI.FocusControl(null);
                    removeAt = i;
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            GUILayout.Space(EditorGUIUtility.labelWidth + 4);
            if (GUILayout.Button(new GUIContent("Add New", GetIcon("icons/add-small")), GUILayout.Height(24)))
            {
                GUI.FocusControl(null);
                PopupWindow.Show(r, new SearchabelStringPopupWindow()
                {
                    width = r.width,
                    buildOptions = buildOptions,
                    onSelection = s =>
                    {
                        list.serializedObject.Update();
                        list.arraySize++;
                        list.GetArrayElementAtIndex(list.arraySize - 1).stringValue = s;
                        list.serializedObject.ApplyModifiedProperties();
                    }
                });

            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            RemoveFromList(list, removeAt);
        }

        public void SimpleStringSearchProperty(SerializedProperty property, string title, List<string> options, bool showClear = true)
        {
            Rect r = EditorGUILayout.GetControlRect(true, GUILayout.ExpandWidth(true));
            GUILayout.Space(-EditorGUIUtility.singleLineHeight);

            GUILayout.BeginHorizontal();
            GUILayout.Label(title, EditorStyles.label, GUILayout.Width(EditorGUIUtility.labelWidth));

            if (GUILayout.Button(property.stringValue, EditorStyles.popup))
            {
                GUI.FocusControl(null);
                PopupWindow.Show(r, new SearchabelStringPopupWindow()
                {
                    width = r.width,
                    objs = options,
                    onSelection = s =>
                    {
                        property.serializedObject.Update();
                        property.stringValue = s;
                        property.serializedObject.ApplyModifiedProperties();
                    }
                });
            }

            if (showClear && !string.IsNullOrEmpty(property.stringValue))
            {
                if (GUILayout.Button("Clear", GUILayout.Width(48)))
                {
                    GUI.FocusControl(null);
                    property.stringValue = string.Empty;
                }
            }

            GUILayout.EndHorizontal();
        }

        public void SimpleStringSearchProperty(SerializedProperty property, string title, Action<List<string>> buildOptions, bool showClear = true, bool endHorizontal = true)
        {
            Rect r = EditorGUILayout.GetControlRect(true, GUILayout.ExpandWidth(true));
            GUILayout.Space(-EditorGUIUtility.singleLineHeight);

            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(title);

            if (GUILayout.Button(property.stringValue, EditorStyles.popup))
            {
                r.x += EditorGUIUtility.labelWidth + 2;
                r.width -= EditorGUIUtility.labelWidth + 2;
                GUI.FocusControl(null);
                PopupWindow.Show(r, new SearchabelStringPopupWindow()
                {
                    width = r.width,
                    buildOptions = buildOptions,
                    onSelection = s =>
                    {
                        property.serializedObject.Update();
                        property.stringValue = s;
                        property.serializedObject.ApplyModifiedProperties();
                    }
                });
            }

            if (showClear && !string.IsNullOrEmpty(property.stringValue))
            {
                if (GUILayout.Button("Clear", GUILayout.Width(48)))
                {
                    GUI.FocusControl(null);
                    property.stringValue = string.Empty;
                }
            }

            if (endHorizontal)
            {
                GUILayout.EndHorizontal();
            }
        }

        public UIColorBlock SimpleUIColorBlock(string propertyName)
        {
            UIColorBlock cb = new UIColorBlock();
            SerializedProperty property = serializedObject.FindProperty(propertyName);

            cb.normalColor = EditorGUILayout.ColorField("Normal", property.FindPropertyRelative("normalColor").colorValue);
            cb.selectedColor = EditorGUILayout.ColorField("Selected", property.FindPropertyRelative("selectedColor").colorValue);
            cb.highlightedColor = EditorGUILayout.ColorField("Highlighted", property.FindPropertyRelative("highlightedColor").colorValue);
            cb.pressedColor = EditorGUILayout.ColorField("Pressed", property.FindPropertyRelative("pressedColor").colorValue);
            cb.disabledColor = EditorGUILayout.ColorField("Disabled", property.FindPropertyRelative("disabledColor").colorValue);
            cb.colorMultiplier = EditorGUILayout.FloatField("Color Multiplier", property.FindPropertyRelative("colorMultiplier").floatValue);
            cb.fadeDuration = EditorGUILayout.FloatField("Fade Duration", property.FindPropertyRelative("fadeDuration").floatValue);

            return cb;
        }

        public UIToggleColorBlock SimpleUIToggleColorBlock(string propertyName)
        {
            UIToggleColorBlock cb = new UIToggleColorBlock();
            SerializedProperty property = serializedObject.FindProperty(propertyName);

            cb.normalColor = EditorGUILayout.ColorField("Normal", property.FindPropertyRelative("normalColor").colorValue);
            cb.highlightedColor = EditorGUILayout.ColorField("Highlighted", property.FindPropertyRelative("highlightedColor").colorValue);
            cb.pressedColor = EditorGUILayout.ColorField("Pressed", property.FindPropertyRelative("pressedColor").colorValue);
            cb.disabledColor = EditorGUILayout.ColorField("Disabled", property.FindPropertyRelative("disabledColor").colorValue);
            cb.colorMultiplier = EditorGUILayout.FloatField("Color Multiplier", property.FindPropertyRelative("colorMultiplier").floatValue);
            cb.fadeDuration = EditorGUILayout.FloatField("Fade Duration", property.FindPropertyRelative("fadeDuration").floatValue);

            return cb;
        }

        public T SimpleValue<T>(string propertyName)
        {
            return SimpleValue<T>(serializedObject.FindProperty(propertyName));
        }

        public static T SimpleValue<T>(SerializedProperty property, string relativeName)
        {
            return SimpleValue<T>(property.FindPropertyRelative(relativeName));
        }

        private static T SimpleValue<T>(SerializedProperty prop)
        {
            object result = null;
            Type type = typeof(T);

            if (type == typeof(bool))
            {
                result = prop.boolValue;
            }
            else if (type == typeof(BoundsInt))
            {
                result = prop.boundsIntValue;
            }
            else if (type == typeof(Bounds))
            {
                result = prop.boundsValue;
            }
            else if (type == typeof(Color))
            {
                result = prop.colorValue;
            }
            else if (type == typeof(double))
            {
                result = prop.doubleValue;
            }
            else if (type == typeof(float))
            {
                result = prop.floatValue;
            }
            else if (type == typeof(int))
            {
                result = prop.intValue;
            }
            else if (type == typeof(long))
            {
                result = prop.longValue;
            }
            else if (type == typeof(Quaternion))
            {
                result = prop.quaternionValue;
            }
            else if (type == typeof(RectInt))
            {
                result = prop.rectIntValue;
            }
            else if (type == typeof(Rect))
            {
                result = prop.rectValue;
            }
            else if (type == typeof(string))
            {
                result = prop.stringValue;
            }
            else if (type == typeof(Vector2Int))
            {
                result = prop.vector2IntValue;
            }
            else if (type == typeof(Vector2))
            {
                result = prop.vector2Value;
            }
            else if (type == typeof(Vector3Int))
            {
                result = prop.vector3IntValue;
            }
            else if (type == typeof(Vector3))
            {
                result = prop.vector3Value;
            }
            else if (type == typeof(Vector4))
            {
                result = prop.vector4Value;
            }

            return (T)result;
        }

        public static void SimpleValue<T>(SerializedProperty prop, T value)
        {
            Type type = typeof(T);
            object setValue = value;


            if (type == typeof(bool))
            {
                prop.boolValue = (bool)setValue;
            }
            else if (type == typeof(BoundsInt))
            {
                prop.boundsIntValue = (BoundsInt)setValue;
            }
            else if (type == typeof(Bounds))
            {
                prop.boundsValue = (Bounds)setValue;
            }
            else if (type == typeof(Color))
            {
                prop.colorValue = (Color)setValue;
            }
            else if (type == typeof(double))
            {
                prop.doubleValue = (double)setValue;
            }
            else if (type == typeof(float))
            {
                prop.floatValue = (float)setValue;
            }
            else if (type == typeof(int))
            {
                prop.intValue = (int)setValue;
            }
            else if (type == typeof(long))
            {
                prop.longValue = (long)setValue;
            }
            else if (type == typeof(Quaternion))
            {
                prop.quaternionValue = (Quaternion)setValue;
            }
            else if (type == typeof(RectInt))
            {
                prop.rectIntValue = (RectInt)setValue;
            }
            else if (type == typeof(Rect))
            {
                prop.rectValue = (Rect)setValue;
            }
            else if (type == typeof(string))
            {
                prop.stringValue = (string)setValue;
            }
            else if (type == typeof(Vector2Int))
            {
                prop.vector2IntValue = (Vector2Int)setValue;
            }
            else if (type == typeof(Vector2))
            {
                prop.vector2Value = (Vector2)setValue;
            }
            else if (type == typeof(Vector3Int))
            {
                prop.vector3IntValue = (Vector3Int)setValue;
            }
            else if (type == typeof(Vector3))
            {
                prop.vector3Value = (Vector3)setValue;
            }
            else if (type == typeof(Vector4))
            {
                prop.vector4Value = (Vector4)setValue;
            }
        }

        public static void SimpleValue<T>(SerializedProperty prop, string relativeName, T value)
        {
            SimpleValue<T>(prop.FindPropertyRelative(relativeName), value);
        }

        public void StringList(string propertyName)
        {
            InternalStringList(serializedObject.FindProperty(propertyName), ObjectNames.NicifyVariableName(propertyName));
        }

        public void StringList(string propertyName, string title)
        {
            InternalStringList(serializedObject.FindProperty(propertyName), title);
        }

        public void StringList(SerializedProperty property, string relativeName)
        {
            InternalStringList(property.FindPropertyRelative(relativeName), ObjectNames.NicifyVariableName(relativeName));
        }

        public void StringList(SerializedProperty property, string relativeName, string title)
        {
            InternalStringList(property.FindPropertyRelative(relativeName), title);
        }

        public void SubHeader(string title, string listName = null, Type acceptedType = null, bool preventDuplicates = true)
        {
            SerializedProperty list = serializedObject.FindProperty(listName);
            bool displayList = list != null && acceptedType != null;

            GUILayout.Space(displayList ? 8 : 12);

            if (!displayList)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(title, Styles.SubHeaderStyle);
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                GUILayout.Space(5);
                GUILayout.Label(title, Styles.SubHeaderStyle);
                GUILayout.EndVertical();

                Color res = GUI.color;
                DrawDragDropIcon(res);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                ProcessDragDrop(list, acceptedType, preventDuplicates);
            }

            GUILayout.Space(4);
        }

        #endregion

        #region Private Methods

        private static void DrawDragDropIcon(Color res)
        {
            GUILayout.BeginVertical();
            GUILayout.Space(4);
            GUI.color = Styles.EditorColor;
            GUILayout.Label(new GUIContent(GetIcon("Drop", "Icons/drop"), "Drag and drop here"), GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false), GUILayout.Height(16), GUILayout.Width(18));
            GUI.color = res;
            GUILayout.EndVertical();
        }

        private void InternalStringList(SerializedProperty list, string title)
        {
            SerializedProperty entry;
            int removeAt = -1;

            GUILayout.BeginHorizontal();

            GUILayout.Label(title, GUILayout.Width(EditorGUIUtility.labelWidth - 4));

            GUILayout.BeginVertical();

            GUILayout.Label("(" + list.arraySize + ")");

            for (int i = 0; i < list.arraySize; i++)
            {
                entry = list.GetArrayElementAtIndex(i);
                GUILayout.BeginHorizontal();
                entry.stringValue = GUILayout.TextField(entry.stringValue);
                if (GUILayout.Button(GetIcon("icons/trash-small"), GUILayout.Width(32), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    GUI.FocusControl(null);
                    removeAt = i;
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Add New", GetIcon("icons/add-small")), GUILayout.Height(24)))
            {
                list.arraySize++;
                list.GetArrayElementAtIndex(list.arraySize - 1).stringValue = string.Empty;
                GUI.FocusControl(null);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            RemoveFromList(list, removeAt);
        }

        private bool ProcessDragDrop(SerializedProperty list, Type acceptedType, bool preventDuplicates)
        {
            bool resValue = false;

            var dragAreaGroup = GUILayoutUtility.GetLastRect();
            switch (Event.current.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dragAreaGroup.Contains(Event.current.mousePosition))
                        break;
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (Event.current.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        foreach (var dragged in DragAndDrop.objectReferences)
                        {
                            if (dragged.GetType() == acceptedType || dragged.GetType().BaseType == acceptedType
                                || dragged.GetType().GetNestedTypes().Contains(acceptedType)
                                || (dragged is GameObject @object && @object.GetComponentInChildren(acceptedType) != null))
                            {
                                if (preventDuplicates)
                                {
                                    for (int i = 0; i < list.arraySize; i++)
                                    {
                                        if (list.GetArrayElementAtIndex(i).objectReferenceValue == dragged)
                                        {
                                            return false;
                                        }
                                    }
                                }

                                list.arraySize++;
                                list.GetArrayElementAtIndex(list.arraySize - 1).objectReferenceValue = dragged;
                                resValue = true;
                            }
                        }
                    }
                    serializedObject.ApplyModifiedProperties();
                    Event.current.Use();
                    break;
            }

            return resValue;

        }

        #endregion

    }
}