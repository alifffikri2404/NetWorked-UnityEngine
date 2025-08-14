using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    public class PopupSearchWindow : PopupWindowContent
    {

        #region Fields

        public Action<int> onSelection;
        public float width;
        public int startIndex;

        public List<string> objs;
        private List<string> filteredObjs;
        private int selIndex;
        private string searchValue;
        private Vector2 scrollInfo;
        private bool needsSetFocus;

        #endregion

        #region Unity Methods

        public override void OnGUI(Rect rect)
        {
            if (filteredObjs == null)
            {
                editorWindow.wantsMouseMove = true;
                filteredObjs = new List<string>();
                editorWindow.minSize = editorWindow.maxSize = new Vector2(Mathf.Max(300, width), 202);
                needsSetFocus = true;

                if (startIndex > -1 && startIndex <= objs.Count - 1)
                {
                    searchValue = objs[startIndex];
                }

                UpdateFilter();
            }

            if (Event.current.type == EventType.KeyDown)
            {
                switch (Event.current.keyCode)
                {
                    case KeyCode.UpArrow:
                        if (selIndex > 0)
                        {
                            selIndex -= 1;
                        }
                        editorWindow.Repaint();
                        break;
                    case KeyCode.DownArrow:
                        if (selIndex < filteredObjs.Count - 1)
                        {
                            selIndex += 1;
                        }
                        editorWindow.Repaint();
                        break;
                    case KeyCode.Return:
                    case KeyCode.KeypadEnter:
                        ReturnSelection();
                        return;
                    case KeyCode.Escape:
                        editorWindow.Close();
                        return;
                }
            }

            GUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"), GUILayout.Height(20));
            GUI.SetNextControlName("txtSearch");
            string newSearch = GUILayout.TextField(searchValue, GDTKEditor.GetSearchbarStyle());
            if (GUILayout.Button(string.Empty, GDTKEditor.GetSearchbarCancelStyle()))
            {
                newSearch = string.Empty;
                GUI.FocusControl(null);
            }
            GUILayout.EndHorizontal();

            if (newSearch != searchValue)
            {
                selIndex = 0;
                searchValue = newSearch;
                UpdateFilter();
            }

            scrollInfo = GUILayout.BeginScrollView(scrollInfo, GUILayout.ExpandHeight(true));
            for (int i = 0; i < filteredObjs.Count; i++)
            {
                GUILayout.BeginVertical(i == selIndex ? GDTKEditor.Styles.ListItemHover : GDTKEditor.Styles.ListItem);
                GUILayout.BeginHorizontal();

                GUILayout.Label(filteredObjs[i], i == selIndex ? GDTKEditor.Styles.ListItemTextHover : GDTKEditor.Styles.ListItemText);

                GUILayout.EndHorizontal();
                GUILayout.EndVertical();

                if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                {
                    if (selIndex != i)
                    {
                        selIndex = i;
                        editorWindow.Repaint();
                    }
                }
            }
            GUILayout.EndScrollView();

            if (needsSetFocus)
            {
                GUI.FocusControl("txtSearch");
                needsSetFocus = false;
            }
        }

        #endregion

        #region Private Methods

        private void ReturnSelection()
        {
            onSelection?.Invoke(objs.IndexOf(filteredObjs[selIndex]));
            editorWindow.Close();
        }

        private void UpdateFilter()
        {
            if (searchValue == null) searchValue = string.Empty;
            selIndex = 0;
            filteredObjs.Clear();
            filteredObjs.AddRange(objs.Where(_ => _.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0));
        }

        #endregion

    }
}