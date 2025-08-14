using System.IO;
using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    public class GDTKEditorWindow : EditorWindow
    {

        #region public Methods

        public Vector2 MainContainerBegin(Vector2 scrollPosition)
        {
            GUILayout.BeginVertical();
            GUILayout.Space(4);
            return GUILayout.BeginScrollView(scrollPosition);
        }

        public void MainContainerEnd()
        {
            GUILayout.EndScrollView();
            GUILayout.Space(4);
            GUILayout.EndVertical();
        }

        public string InlineProperty(string value, string title)
        {
            return EditorGUILayout.TextField(new GUIContent(title, null, string.Empty), value);
        }

        public bool SaveScriptableObject(Object target, string prompt, string defaultName, string pathPref)
        {
            string defaultPath = PlayerPrefs.GetString(pathPref, "Assets");
            string path = EditorUtility.SaveFilePanelInProject(prompt, defaultName, "asset", "Select a location to save the item.", defaultPath);
            if (string.IsNullOrEmpty(path)) return false;

            PlayerPrefs.SetString(pathPref, Path.GetDirectoryName(path));
            AssetDatabase.CreateAsset(target, path);
            AssetDatabase.SaveAssets();

            return true;
        }

        public bool SectionGroup(string title, Texture2D icon, bool expand)
        {
            bool resValue = expand;

            // Top spacing
            GUILayout.Space(8);

            // Container start
            GUILayout.BeginHorizontal();

            // Expand collapse icon
            GUILayout.BeginVertical();
            Color res = GUI.color;
            GUILayout.Space(5);
            Texture2D texture = resValue ? GDTKEditor.Styles.ExpandedIcon : GDTKEditor.Styles.CollapsedIcon;
            GUI.color = GDTKEditor.Styles.EditorColor;
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

            GUILayout.Label(title, GDTKEditor.Styles.SectionHeaderStyle);
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();

            // Container End
            GUILayout.EndHorizontal();

            // Toggle
            if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
            {
                resValue = !resValue;
                Repaint();
            }

            GUILayout.Space(4);

            return resValue;
        }

        public void SectionHeader(string title)
        {
            GUILayout.Space(12);
            GUILayout.BeginHorizontal();
            GUILayout.Space(8);
            GUILayout.Label(title, GDTKEditor.Styles.SectionHeaderStyle);
            GUILayout.EndHorizontal();
            GUILayout.Space(4);
        }

        public bool SectionToggle(ref int displayFlags, int flag, string title, Texture2D icon = null)
        {
            bool hasFlag = (displayFlags & flag) == flag;
            bool result = SectionGroup(title, icon, hasFlag);

            if (result != hasFlag)
            {
                displayFlags = result ? displayFlags | flag : displayFlags & ~flag;
            }

            return hasFlag;
        }

        public bool SimpleEditorBool(string title, bool value)
        {
            return EditorGUILayout.Toggle(title, value);
        }

        public int SimpleEditorDropDown(string title, int value, string[] options)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title, GUILayout.ExpandWidth(true), GUILayout.MinWidth(EditorGUIUtility.labelWidth));
            int result = EditorGUILayout.Popup(value, options);
            GUILayout.EndHorizontal();
            return result;
        }

        public float SimpleEditorFloat(string title, float value)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title, GUILayout.ExpandWidth(true), GUILayout.MinWidth(EditorGUIUtility.labelWidth));
            float result = EditorGUILayout.FloatField(value);
            GUILayout.EndHorizontal();
            return result;
        }

        public GameObject SimpleEditorGameObject(string title, GameObject value)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title, GUILayout.ExpandWidth(true), GUILayout.MinWidth(EditorGUIUtility.labelWidth));
            GameObject result = (GameObject)EditorGUILayout.ObjectField(value, typeof(GameObject), true);
            GUILayout.EndHorizontal();
            return result;
        }

        public int SimpleEditorInt(string title, int value)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title, GUILayout.ExpandWidth(true), GUILayout.MinWidth(EditorGUIUtility.labelWidth));
            int result = EditorGUILayout.IntField(value);
            GUILayout.EndHorizontal();
            return result;
        }

        public void SimpleEditorLabel(string text)
        {
            GUILayout.Label(text, GUILayout.ExpandWidth(true), GUILayout.MinWidth(EditorGUIUtility.labelWidth));
        }

        public void SimpleEditorLabel(string title, string value)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label(title, GUILayout.ExpandWidth(true), GUILayout.MinWidth(EditorGUIUtility.labelWidth));
            GUI.enabled = false;
            EditorGUILayout.TextField(value);
            GUI.enabled = true;
            GUILayout.EndHorizontal();
        }

        public Object SimpleEditorObject(string title, Object value, System.Type type)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title, GUILayout.ExpandWidth(true), GUILayout.MinWidth(EditorGUIUtility.labelWidth));
            Object result = EditorGUILayout.ObjectField(value, type, true);
            GUILayout.EndHorizontal();
            return result;
        }

        public Sprite SimpleEditorSprite(string title, Sprite value)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title, GUILayout.ExpandWidth(true), GUILayout.MinWidth(EditorGUIUtility.labelWidth));
            Sprite result = (Sprite)EditorGUILayout.ObjectField(value, typeof(Sprite), true);
            GUILayout.EndHorizontal();
            return result;
        }

        public string SimpleEditorText(string title, string value)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title, GUILayout.ExpandWidth(true), GUILayout.MinWidth(EditorGUIUtility.labelWidth));
            string result = EditorGUILayout.TextField(value);
            GUILayout.EndHorizontal();
            return result;
        }

        public void SimpleList(SerializedObject serializedObject, string listName)
        {
            SerializedProperty list = serializedObject.FindProperty(listName);

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
                GUILayout.Label("Right-click item to remove", GDTKEditor.Styles.FooterStyle);
            }
            else
            {
                GUILayout.Label("{Empty}", GDTKEditor.Styles.FooterStyle);
            }
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add"))
            {
                list.arraySize++;
            }
            if (GUILayout.Button("Clear")) { list.arraySize = 0; }
            GUILayout.EndHorizontal();
        }

        public void SimpleProperty(SerializedObject serializedObject, string propertyName)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(propertyName));
        }

        public void SimpleWrappedText(string text)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(8);
            GUILayout.Label(text, GDTKEditor.Styles.WrappedTextStyle);
            GUILayout.Space(8);
            GUILayout.EndHorizontal();
        }

        #endregion

        #region Private Methods

        private static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
            {
                pix[i] = col;
            }

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        #endregion

    }
}