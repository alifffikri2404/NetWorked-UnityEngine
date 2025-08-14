using NullSave.GDTK.JSON;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(ActionSequence))]
    public class ActionSequenceEditor : GDTKEditor
    {

        #region Fields

        private ActionSequence myTarget;
        private Rect buttonRect;
        private UniversalObjectEditorInfo _plugins;
        private bool doImport, doExport;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if (target is ActionSequence sequence)
            {
                myTarget = sequence;
                if (myTarget.plugins == null) myTarget.plugins = new List<ActionSequenceWrapper>();
            }

            _plugins = new UniversalObjectEditorInfo();
        }

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            if (Application.isPlaying)
            {
                EditorGUI.BeginDisabledGroup(myTarget.isStarted);
                if (GUILayout.Button("Play Sequence"))
                {
                    myTarget.Play();
                }
                EditorGUI.EndDisabledGroup();
            }

            SectionHeader("Behavior");
            SimpleProperty("playOnStart");
            SimpleProperty("playOnEnable");
            SimpleProperty("remoteTarget");

            SectionHeader("Sequence");
            DrawList();
            if (GUILayout.Button("Add Plugin"))
            {
                PopupWindow.Show(buttonRect, new ActionPluginPicker() { target = myTarget.plugins, width = buttonRect.width });
            }
            if (Event.current.type == EventType.Repaint) buttonRect = GUILayoutUtility.GetLastRect();

            GUILayout.Space(12);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Import", GUILayout.Height(26)))
            {
                doImport = true;
            }
            GUILayout.Space(16);
            if (GUILayout.Button("Export", GUILayout.Height(26)))
            {
                doExport = true;
            }
            GUILayout.EndHorizontal();

            _plugins.UpdateDragPosition(myTarget.plugins, this, Styles.Redline);
            MainContainerEnd();

            if (doImport)
            {
                bool doClear = false;
                doImport = false;
                string importPath = EditorUtility.OpenFilePanel("Import Action Sequence", Application.dataPath, "json");
                if (!string.IsNullOrEmpty(importPath))
                {
                    if (EditorUtility.DisplayDialog("Action Sequence", "Clear existing data before importing?", "Yes", "No"))
                    {
                        doClear = true;
                    }
                    jsonActionSequence sequence = SimpleJson.FromJSON<jsonActionSequence>(File.ReadAllText(importPath));
                    sequence.ToModel(myTarget, doClear);
                }
            }

            if (doExport)
            {
                doExport = false;
                string exportPath = EditorUtility.SaveFilePanel("Export Action Sequence", Application.dataPath, "ActionSequence", "json");
                if (!string.IsNullOrEmpty(exportPath))
                {
                    File.WriteAllText(exportPath, myTarget.ExportToJSON());
                    AssetDatabase.Refresh();
                }
            }
        }

        public override bool RequiresConstantRepaint()
        {
            return _plugins.isDragging || myTarget.isStarted;
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
                ActionSequenceWrapper wrapper = myTarget.plugins[i];
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