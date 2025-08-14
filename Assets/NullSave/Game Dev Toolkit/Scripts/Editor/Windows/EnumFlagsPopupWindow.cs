using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    public class EnumFlagsPopupWindow : PopupWindowContent
    {
        #region Fields

        public Action<int> onSelection;
        public float width;

        public Enum value;
        private Enum[] flags;
        private List<Enum> filteredObjs;

        private string searchValue;

        private int result;
        private float minWidth;
        private bool needsWidth;
        private Vector2 scroll;

        #endregion

        #region Unity Methods

        public override void OnGUI(Rect rect)
        {
            bool wantsRepaint = false;
            int curFlag;
            int totalVal = 0;

            if (flags == null)
            {
                Array enumOptions = Enum.GetValues(value.GetType());
                flags = new Enum[enumOptions.Length];
                int index = 0;
                foreach(var opt in enumOptions)
                {
                    flags[index++] = (Enum)opt;
                }
                result = (int)Convert.ChangeType(value, typeof(int));
                needsWidth = true;

                filteredObjs = new List<Enum>();
                UpdateFilter();
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
                searchValue = newSearch;
                UpdateFilter();
            }


            scroll = GUILayout.BeginScrollView(scroll);

            for (int i = 0; i < filteredObjs.Count; i++)
            {
                curFlag = (int)Convert.ChangeType(filteredObjs[i], typeof(int));
                totalVal += curFlag;
                if (curFlag != 0)
                {
                    GUILayout.BeginVertical(GUILayout.Height(32));
                    GUILayout.BeginHorizontal();
                    if (value.HasFlag(filteredObjs[i]))
                    {
                        GUILayout.BeginVertical();
                        GUILayout.Space(8);
                        GUILayout.Label(GDTKEditor.GetIcon("icons/check"), GUILayout.Height(16));
                        GUILayout.EndVertical();
                        GUILayout.Space(-12);
                    }
                    else
                    {
                        GUILayout.Space(32);
                    }
                    GUILayout.BeginVertical();
                    GUILayout.Space(8);
                    GUILayout.Label(filteredObjs[i].ToString());
                    if (needsWidth && Event.current.type == EventType.Repaint)
                    {
                       minWidth = Mathf.Max(minWidth, GUILayoutUtility.GetLastRect().width);
                    }
                    GUILayout.EndVertical();
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    if (Event.current.type == EventType.MouseDown && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                    {
                        int newVal = (int)Convert.ChangeType(value, typeof(int));
                        newVal ^= curFlag;

                        value = (Enum)Enum.ToObject(value.GetType(), newVal);
                        wantsRepaint = true;
                        result = newVal;
                    }
                }
            }

            GUILayout.EndScrollView();

            GUILayout.BeginVertical(GDTKEditor.Styles.SlimBox, GUILayout.Height(36));
            GUILayout.Space(6);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Done", GUILayout.Height(24)))
            {
                GUI.FocusControl(null);
                ReturnSelection();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Select All", GUILayout.Height(24)))
            {
                GUI.FocusControl(null);
                value = (Enum)Enum.ToObject(value.GetType(), totalVal);
                result = totalVal;
                wantsRepaint = true;
            }
            GUILayout.Space(8);
            if (GUILayout.Button("Select None", GUILayout.Height(24)))
            {
                GUI.FocusControl(null);
                value = (Enum)Enum.ToObject(value.GetType(), 0);
                result = 0;
                wantsRepaint = true;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            if (Event.current.type == EventType.KeyDown)
            {
                switch (Event.current.keyCode)
                {
                    case KeyCode.Return:
                    case KeyCode.KeypadEnter:
                        ReturnSelection();
                        break;
                    case KeyCode.Escape:
                        editorWindow.Close();
                        break;
                }
            }

            if(needsWidth && Event.current.type == EventType.Repaint)
            {
                editorWindow.minSize = editorWindow.maxSize = new Vector2(Mathf.Max(260, 56 + minWidth), Mathf.Min(600, 64 + (flags.Length * 32)));
                needsWidth = false;
            }

            if (wantsRepaint) editorWindow.Repaint();
        }

        #endregion

        #region Private Methods

        private void ReturnSelection()
        {
            onSelection?.Invoke(result);
            editorWindow.Close();
        }

        private void UpdateFilter()
        {
            if (searchValue == null) searchValue = string.Empty;
            filteredObjs.Clear();
            filteredObjs.AddRange(flags.Where(_ => _.ToString().IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0));
        }

        #endregion

    }
}