using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(FlexList))]
    public class FlexListEditor : GDTKEditor
    {

        #region Fields

        private FlexList myTarget;
        private Rect buttonRect;
        private UniversalObjectEditorInfo _plugins;
        private readonly string compat = typeof(FlexListOption).ToString();
        private ReorderableList optionList;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if (target is FlexList sequence)
            {
                myTarget = sequence;
                if (myTarget.plugins == null) myTarget.plugins = new List<SAFPluginWrapper>();
            }

            _plugins = new UniversalObjectEditorInfo();

            optionList = new ReorderableList(serializedObject, serializedObject.FindProperty("m_options"), true, true, true, true);
            optionList.elementHeight = (EditorGUIUtility.singleLineHeight + 2) * 2;
            optionList.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "Options"); };
            optionList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = optionList.serializedProperty.GetArrayElementAtIndex(index);

                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("text"), new GUIContent("Text", null, string.Empty));
                rect.y += EditorGUIUtility.singleLineHeight + 2;
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("image"), new GUIContent("Image", null, string.Empty));
                rect.y += EditorGUIUtility.singleLineHeight + 2;
            };

        }

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            GUILayout.Space(12);
            SectionHeader("Behavior", GetIcon("icons/behavior"));
            SimpleProperty("scrollRect", "Scroll Rect");
            SimpleProperty("gridTemplate", "Grid UI Prefab");
            SimpleProperty("listTemplate", "List UI Prefab");
            optionList.DoLayoutList();

            GUILayout.Space(12);
            SectionHeader("Navigation", GetIcon("icons/navigation"));
            SimpleProperty("autoSelectFirstItem");
            SimpleProperty("navigation");

            GUILayout.Space(12);
            SectionHeader("UI Layout", GetIcon("icons/ui"));
            SimpleProperty("orientation");
            SimpleProperty("layout", "Default Layout");
            SimpleProperty("cellSize", "Grid Cell Size");
            SimpleProperty("cellSpacing", "Grid Cell Spacing");
            EditorGUILayout.PrefixLabel("Layout Padding");
            SerializedProperty padding = serializedObject.FindProperty("padding");
            EditorGUI.indentLevel++;
            SimpleProperty(padding, "m_Left");
            SimpleProperty(padding, "m_Right");
            SimpleProperty(padding, "m_Top");
            SimpleProperty(padding, "m_Bottom");
            EditorGUI.indentLevel--;
            SimpleProperty("listSpacing");

            GUILayout.Space(12);
            SectionHeader("Sort & Filter Plugins", GetIcon("icons/filter"));
            DrawList();
            if (GUILayout.Button("Add Plugin"))
            {
                PopupWindow.Show(buttonRect, new SAFPluginPicker() { target = myTarget.plugins, width = buttonRect.width, listType = compat });
            }
            if (Event.current.type == EventType.Repaint) buttonRect = GUILayoutUtility.GetLastRect();

            GUILayout.Space(12);
            SectionHeader("Events", GetIcon("icons/event"));
            SimpleProperty("onSelectionChanged");

            MainContainerEnd();
        }

        #endregion

        #region Private Methods

        private void DrawList()
        {
            Color resColor = GUI.contentColor;
            GUIStyle style;
            UniversalObjectEditorItemInfo itemInfo;
            int toggleIndex = -1;
            int removeAt = -1;

            float maxWidth = EditorGUIUtility.currentViewWidth - 24 - 40;

            for (int i = 0; i < myTarget.plugins.Count; i++)
            {
                SAFPluginWrapper wrapper = myTarget.plugins[i];
                itemInfo = _plugins.GetInfo(wrapper.instanceId);
                if (itemInfo.editor == null)
                {
                    itemInfo.editor = UniversalPluginEditor.CreateEditor(wrapper.plugin);
                    wrapper.serializationData = SimpleJson.ToJSON(wrapper.plugin);
                }

                // Check drag
                if (_plugins.isDragging && _plugins.curIndex == i)
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
                    _plugins.BeginDrag(i, this, ref itemInfo);
                    GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);
                }

                // Title
                style = new GUIStyle(itemInfo.isExpanded ? Styles.ButtonMidPressed : Styles.ButtonMid);
                GUILayout.BeginHorizontal(style, GUILayout.Height(22), GUILayout.MaxWidth(maxWidth));
                GUILayout.Space(2);
                GUILayout.Label(wrapper.plugin == null ? null : wrapper.plugin.icon, GUILayout.Height(16), GUILayout.Width(16));
                GUILayout.Space(2);
                GUILayout.Label(wrapper.plugin == null ? "{Missing Plugin}" : wrapper.plugin.titlebarText, Styles.TitleTextStyle, GUILayout.Height(15));
                GUILayout.EndHorizontal();
                if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
                {
                    toggleIndex = i;
                }

                // Delete
                GUI.contentColor = Styles.EditorColor;
                GUILayout.Label(GetIcon("Trash", "icons/trash"), Styles.ButtonRight, GUILayout.Width(22), GUILayout.Height(22));
                GUI.contentColor = resColor;
                if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
                {
                    removeAt = i;
                }

                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();

                if (itemInfo.isExpanded)
                {
                    GUILayout.Space(-5);
                    GUILayout.BeginVertical("box");
                    itemInfo.editor?.OnInspectorGUI();

                    GUILayout.EndVertical();
                }

                if (itemInfo.editor != null && itemInfo.editor.IsDirty)
                {
                    itemInfo.editor.ApplyUpdates(ref wrapper.serializationData);
                    EditorUtility.SetDirty(target);
                }
            }

            if (toggleIndex > -1)
            {
                _plugins.items[myTarget.plugins[toggleIndex].instanceId].isExpanded = !_plugins.items[myTarget.plugins[toggleIndex].instanceId].isExpanded;
                if (_plugins.items[myTarget.plugins[toggleIndex].instanceId].isExpanded)
                {
                    _plugins.items[myTarget.plugins[toggleIndex].instanceId].editor?.OnEnable();
                }
            }

            if (removeAt > -1)
            {
                _plugins.items.Remove(myTarget.plugins[removeAt].instanceId);
                myTarget.plugins.RemoveAt(removeAt);
                EditorUtility.SetDirty(target);
            }
        }

        #endregion

    }
}