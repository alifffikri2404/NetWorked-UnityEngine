using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    public class LocalizationWizard : GDTKEditorWindow
    {

        #region Constants

        private const string TITLE = "Localize Wizard";

        #endregion

        #region Fields

        private Vector2 windowScroll, panelScroll;
        private TextAsset activeTextAsset;

        private List<string> languages, entryIds;
        private Dictionary<string, string> dictionary;
        private TextEncoding encoding;
        private string newLanguage, newEntry;
        private int lngSel, entrySel;
        private bool allEntries;

        // search
        private List<TextAsset> validAssets;

        #endregion

        #region Properties

        private TextAsset ActiveTextAsset
        {
            get { return activeTextAsset; }
            set
            {
                if (activeTextAsset == value) return;
                activeTextAsset = value;
                if (value != null)
                {
                    ValidCSV = value.text.ToLower().StartsWith("localization_key,");
                    if (ValidCSV)
                    {
                        LoadTextAsset();
                    }
                }
            }
        }

        private bool ValidCSV { get; set; }

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            allEntries = true;
        }

        private void OnGUI()
        {
            if (!ValidCSV)
            {
                GetActiveAsset();
            }

            if ((dictionary == null || dictionary.Count == 0) && (entryIds != null && entryIds.Count > 0))
            {
                LoadTextAsset();
            }

            if (entryIds == null || entrySel >= entryIds.Count) entrySel = 0;
            windowScroll = MainContainerBegin(windowScroll);

            DrawHeader();
            DrawBody();
            DrawFooter();

            MainContainerEnd();
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        #endregion

        #region Public Methods

        [MenuItem("Tools/GDTK/Localize Wizard", false, 0)]
        public static void ShowWindow()
        {
            LocalizationWizard w = GetWindow<LocalizationWizard>(TITLE);

            w.titleContent = new GUIContent(TITLE, GDTKEditor.GetIcon("LocalizationIcon", "Icons/localization-winicon"));
            w.minSize = new Vector2(400, 500);
            w.maxSize = new Vector2(800, 600);

            float scale = 1;
            if (Screen.dpi >= 144)
            {
                scale = 0.5f;
            }
            else if (Screen.dpi >= 120)
            {
                scale = 0.75f;
            }
            w.position = new Rect((Screen.currentResolution.width * scale - w.maxSize.x * scale) / 2, (Screen.currentResolution.height * scale - w.maxSize.y * scale) / 2, w.maxSize.x, w.maxSize.y);
            w.wantsMouseMove = true;
        }

        [MenuItem("Tools/GDTK/Highlight Localize Settings")]
        public static void ShowSettings()
        {
            LocalizationSettings[] settings = Resources.LoadAll<LocalizationSettings>(string.Empty);
            if (settings != null && settings.Length > 0)
            {
                Selection.activeObject = settings[0];
                return;
            }

            if (EditorUtility.DisplayDialog("NullSave", "No settings for Localize were found. Would you like to create a settings file now?", "Yes", "No"))
            {
                LocalizationSettings asset = CreateInstance<LocalizationSettings>();

                AssetDatabase.CreateAsset(asset, "Assets/LocalizationSettings.asset");
                AssetDatabase.SaveAssets();

                EditorUtility.FocusProjectWindow();

                Selection.activeObject = asset;
            }
        }

        #endregion

        #region Private Methods

        private void DrawBody()
        {
            if (ActiveTextAsset == null || !ValidCSV)
            {
                DrawFind();
                return;
            }

            GUILayout.Space(8);

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.MaxWidth(250));

            GUILayout.Label("Languages", GDTKEditor.Styles.SubHeaderStyle);
            GUILayout.BeginHorizontal();
            newLanguage = EditorGUILayout.TextField(newLanguage);
            GUILayout.BeginVertical();
            GUILayout.Space(-1);
            if (GUILayout.Button("Add"))
            {
                if (!languages.Contains(newLanguage))
                {
                    languages.Add(newLanguage);
                    foreach (string id in entryIds)
                    {
                        dictionary.Add(newLanguage + "_" + id, string.Empty);
                    }
                }

                newLanguage = string.Empty;
                GUI.FocusControl("Clear");
                EditorUtility.SetDirty(this);
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            DrawList(languages, ref lngSel, false);

            GUILayout.Label("Entries", GDTKEditor.Styles.SubHeaderStyle);
            GUILayout.BeginHorizontal();
            newEntry = EditorGUILayout.TextField(newEntry);
            GUILayout.BeginVertical();
            GUILayout.Space(-1);
            if (GUILayout.Button("Add"))
            {
                if (!entryIds.Contains(newEntry))
                {
                    entryIds.Add(newEntry);
                    foreach (string language in languages)
                    {
                        dictionary.Add(language + "_" + newEntry, string.Empty);
                    }
                }

                newEntry = string.Empty;
                GUI.FocusControl("Clear");
                EditorUtility.SetDirty(this);
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            DrawList(entryIds, ref entrySel, true);

            GUILayout.EndVertical();

            GUILayout.BeginHorizontal(GUILayout.MaxWidth(540));
            panelScroll = GUILayout.BeginScrollView(panelScroll);
            if (entryIds.Count > 0)
            {
                DrawEntries();
            }
            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();

            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draw list of entries
        /// </summary>
        private void DrawEntries()
        {
            string realKey;
            if (dictionary == null) return;

            if (allEntries)
            {
                for (int i = 0; i < languages.Count; i++)
                {
                    GUILayout.Label(entryIds[entrySel] + " [" + languages[i] + "]", GDTKEditor.Styles.SubHeaderStyle);
                    realKey = languages[i] + "_" + entryIds[entrySel];
                    if (!dictionary.ContainsKey(realKey))
                    {
                        dictionary.Add(realKey, string.Empty);
                    }
                    dictionary[realKey] = GUILayout.TextArea(dictionary[realKey]);
                    GUILayout.Space(6);
                }
            }
            else
            {
                GUILayout.Label(entryIds[entrySel] + " [" + languages[lngSel] + "]", GDTKEditor.Styles.SubHeaderStyle);
                realKey = languages[lngSel] + "_" + entryIds[entrySel];
                if (!dictionary.ContainsKey(realKey))
                {
                    dictionary.Add(realKey, string.Empty);
                }
                dictionary[realKey] = GUILayout.TextArea(dictionary[realKey]);
            }
        }

        private void DrawFind()
        {
            if (validAssets == null)
            {
                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Find Asset Resource", GUILayout.Height(28)))
                {

                    validAssets = new List<TextAsset>();
                    TextAsset[] assets = Resources.LoadAll<TextAsset>(string.Empty);
                    foreach (TextAsset asset in assets)
                    {
                        if (asset.text.ToLower().StartsWith("localization_key,"))
                        {
                            validAssets.Add(asset);
                        }
                    }

                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            else
            {
                foreach (TextAsset asset in validAssets)
                {
                    if (GUILayout.Button(asset.name))
                    {
                        Selection.activeObject = asset;
                    }
                }
            }
        }

        private void DrawFooter()
        {
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();

            GUILayout.BeginHorizontal();
            if (ActiveTextAsset != null && ValidCSV)
            {
                if (GUILayout.Button("  Save  ", GUILayout.Height(24)))
                {
                    Save();
                }
            }
            else
            {
                if (GUILayout.Button("  Create New  ", GUILayout.Height(24)))
                {
                    string defaultPath = PlayerPrefs.GetString("LocalizationFiles", string.Empty);
                    string path = EditorUtility.SaveFilePanelInProject("Create Localize File", "NewLocalization", "txt", "Select a location to create your new localization file.", defaultPath);
                    if (!string.IsNullOrEmpty(path))
                    {
                        PlayerPrefs.SetString("LocalizationFiles", Path.GetDirectoryName(path));
                        File.WriteAllText(Path.Combine(Application.dataPath, "../" + path), "Localization_Key,English\r\ndefault,", encoding == TextEncoding.UTF8 ? Encoding.UTF8 : Encoding.UTF32);
                        AssetDatabase.Refresh();
                        ActiveTextAsset = (TextAsset)AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset));
                        Selection.activeObject = ActiveTextAsset;

                    }
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(ActiveTextAsset == null || !ValidCSV);
            if (GUILayout.Button("Export", GDTKEditor.Styles.ButtonLeft, GUILayout.Height(24)))
            {
                Export();
            }
            if (GUILayout.Button("Import", GDTKEditor.Styles.ButtonRight, GUILayout.Height(24)))
            {
                Import();
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();

            GUILayout.EndHorizontal();
        }

        private void DrawHeader()
        {
            GUILayout.BeginVertical();
            if (ActiveTextAsset != null && ValidCSV)
            {
                GUILayout.BeginHorizontal();
                encoding = (TextEncoding)EditorGUILayout.EnumPopup("Encoding", encoding);
                GUILayout.FlexibleSpace();
                GUILayout.Label("Show all languages for entry on one page", GUILayout.ExpandWidth(false));
                allEntries = EditorGUILayout.Toggle("", allEntries);
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.Label("Select or Create a TextAsset to continue.", GDTKEditor.Styles.WrappedTextStyle);
            }
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Draw string list in a listbox
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selEntry"></param>
        /// <param name="isEntry"></param>
        private void DrawList(List<string> source, ref int selEntry, bool isEntry)
        {
            GUILayout.BeginVertical("box", GUILayout.MaxHeight(200), GUILayout.ExpandHeight(true));
            GUIStyle regular = new GUIStyle("label");
            GUIStyle selected = new GUIStyle("label");
            selected.normal.textColor = Color.black;
            selected.normal.background = new Texture2D(1, 1);

            for (int i = 0; i < source.Count; i++)
            {
                if (GUILayout.Button(source[i], i == selEntry ? selected : regular, GUILayout.ExpandWidth(true)))
                {
                    selEntry = i;
                    GUI.FocusControl("Clear");
                    EditorUtility.SetDirty(this);
                }
            }
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Delete"))
            {
                string selVal = source[selEntry];
                if (isEntry)
                {
                    foreach (string language in languages)
                    {
                        dictionary.Remove(language + "_" + selVal);
                    }
                }
                else
                {
                    foreach (string id in entryIds)
                    {
                        dictionary.Remove(selVal + "_" + id);
                    }
                }
                source.RemoveAt(selEntry);
                selEntry = 0;
            }
            GUILayout.EndHorizontal();
        }

        private void Export()
        {
            string defaultPath = PlayerPrefs.GetString("LocalizationExport", string.Empty);
            string path = EditorUtility.SaveFilePanel("Export Localize File", defaultPath, ActiveTextAsset.name, "csv");
            if (!string.IsNullOrEmpty(path))
            {
                string result = "Localization_Key";
                foreach (string language in languages)
                {
                    result += "," + language;
                }

                foreach (string id in entryIds)
                {
                    result += "\r\n" + id;
                    foreach (string language in languages)
                    {
                        result += "," + formatData(dictionary[language + "_" + id]);
                    }
                }

                File.WriteAllText(path, result, encoding == TextEncoding.UTF8 ? Encoding.UTF8 : Encoding.UTF32);
                EditorUtility.DisplayDialog("NullSave", "Localize data exported", "OK");
                AssetDatabase.Refresh();
            }
        }

        private string formatData(string value)
        {
            value = value.Replace("\"", "\"\"");
            value = value.Replace("<", "&lt;");
            value = value.Replace(">", "&gt;");
            value = value.Replace("\r\n", "<br/>");
            if (value.IndexOf(',') >= 0) value = "\"" + value + "\"";

            if (value.Contains("\"") && value[0] != '"' && value[value.Length - 1] != '"')
            {
                value = "\"" + value + "\"";
            }

            return value;
        }

        private void GetActiveAsset()
        {
            if (Selection.activeObject != null && Selection.activeObject is TextAsset asset)
            {
                ActiveTextAsset = asset;
            }
            else
            {
                ActiveTextAsset = null;
            }
        }

        private void Import()
        {
            string defaultPath = PlayerPrefs.GetString("LocalizationImport", string.Empty);
            string path = EditorUtility.OpenFilePanel("Import Localize File", defaultPath, "csv");
            if (!string.IsNullOrEmpty(path))
            {

                // Load data
                string[] columns;
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (CsvParser.CsvReader csv = new CsvParser.CsvReader(fs, encoding == TextEncoding.UTF8 ? Encoding.UTF8 : Encoding.UTF32))
                    {
                        // Read header for languages
                        languages = new List<string>();
                        csv.MoveNext();
                        columns = csv.Current.ToArray();
                        for (int i = 1; i < columns.Length; i++)
                        {
                            languages.Add(columns[i]);
                        }

                        entryIds = new List<string>();
                        dictionary = new Dictionary<string, string>();
                        while (csv.MoveNext())
                        {
                            columns = csv.Current.ToArray();
                            entryIds.Add(columns[0]);
                            for (int i = 1; i < columns.Length; i++)
                            {
                                dictionary.Add(languages[i - 1] + "_" + columns[0], columns[i]);
                            }
                        }
                    }
                }
            }
        }

        private void LoadTextAsset()
        {
            try
            {
                // Load data
                string[] columns;
                using (MemoryStream ms = new MemoryStream(ActiveTextAsset.bytes))
                {
                    using (CsvParser.CsvReader csv = new CsvParser.CsvReader(ms, encoding == TextEncoding.UTF8 ? Encoding.UTF8 : Encoding.UTF32))
                    {
                        // Read header for languages
                        languages = new List<string>();
                        csv.MoveNext();
                        columns = csv.Current.ToArray();
                        for (int i = 1; i < columns.Length; i++)
                        {
                            languages.Add(columns[i]);
                        }

                        entryIds = new List<string>();
                        dictionary = new Dictionary<string, string>();
                        while (csv.MoveNext())
                        {
                            columns = csv.Current.ToArray();
                            entryIds.Add(columns[0]);
                            for (int i = 1; i < columns.Length; i++)
                            {
                                dictionary.Add(languages[i - 1] + "_" + columns[0], columns[i].Replace("<br/>", "\r\n").Replace("&lt;", "<").Replace("&gt;", ">"));
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                StringExtensions.LogWarning("LocalizationWizard.cs", "LoadTextAsset", "Could not complete localization load: " + ex.Message);
                ActiveTextAsset = null;
            }
        }

        private void Save()
        {
            string result = "Localization_Key";
            string key;

            foreach (string language in languages)
            {
                result += "," + language;
            }

            foreach (string id in entryIds)
            {
                result += "\r\n" + id;
                foreach (string language in languages)
                {
                    key = language + "_" + id;
                    if (!dictionary.ContainsKey(key))
                    {
                        dictionary.Add(language + "_" + id, string.Empty);
                    }
                    result += "," + formatData(dictionary[key]);
                }
            }

            File.WriteAllText(AssetDatabase.GetAssetPath(ActiveTextAsset), result, encoding == TextEncoding.UTF8 ? Encoding.UTF8 : Encoding.UTF32);
            EditorUtility.SetDirty(ActiveTextAsset);
            EditorUtility.DisplayDialog("NullSave", "Asset saved", "OK");
            AssetDatabase.Refresh();
        }

        #endregion

    }
}