using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    public class UniversalObjectEditorInfo
    {

        #region Fields

        public Dictionary<string, UniversalObjectEditorItemInfo> items;

        public bool isDragging;
        public Vector2 startPos;
        public int startIndex;
        public int curIndex;

        #endregion

        #region Public Methods

        public void BeginDrag(int index, Editor editor, ref UniversalObjectEditorItemInfo itemInfo)
        {
            if (!isDragging)
            {
                isDragging = true;
                startIndex = index;
                itemInfo.isDragging = true;
                curIndex = index;
                startPos = Event.current.mousePosition;
                if (editor != null) editor.Repaint();
            }
        }

        public UniversalObjectEditorItemInfo GetInfo(string key)
        {
            if (items == null) items = new Dictionary<string, UniversalObjectEditorItemInfo>();

            if (!items.ContainsKey(key))
            {
                items.Add(key, new UniversalObjectEditorItemInfo());
            }
            return items[key];
        }

        public void UpdateDragPosition<T>(List<T> list, Editor editor, GUIStyle dividerStyle)
        {
            // Check new drag index
            if (isDragging)
            {
                if (curIndex >= list.Count)
                {
                    GUILayout.Space(2);
                    GUILayout.Label(string.Empty, dividerStyle, GUILayout.Height(1), GUILayout.ExpandWidth(true));
                    GUILayout.Space(2);
                }

                if (Event.current.type == EventType.MouseDrag)
                {
                    curIndex = startIndex + (int)((Event.current.mousePosition.y - startPos.y) / 22);
                    if (curIndex < 0) curIndex = 0;
                    if (curIndex >= list.Count) curIndex = list.Count - 1;
                }
                else if (Event.current.type == EventType.MouseUp)
                {
                    isDragging = false;
                    foreach (var entry in items)
                    {
                        entry.Value.isDragging = false;
                    }

                    if (curIndex >= list.Count) curIndex = list.Count - 1;
                    if (startIndex != curIndex)
                    {
                        T source = list[startIndex];
                        T dest = list[curIndex];

                        list[startIndex] = dest;
                        list[curIndex] = source;
                    }

                    editor.Repaint();
                }
            }
        }

        #endregion

    }
}
