using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    public class Localize 
    {

        #region Fields

        public static LanguageChanged onLanguageChanged = new LanguageChanged();
        public static UnityEvent onSettingsChanged = new UnityEvent();

        private static string currentLanguage;
        private static Dictionary<string, string> dictionary;
        private static int lngIndex;

        private static DictionaryLookupMode lookupMode;
        private static DictionarySource sourceType;

        private static bool initializing;

        #endregion

        #region Properties

        /// <summary>
        /// Name of the asset containing the localization data inside of the specified BundleName
        /// </summary>
        public static string BundleAssetName { get; set; }

        /// <summary>
        /// Name of the AssetBundle to load when retrieving localization data
        /// </summary>
        public static string BundleName { get; set; }

        /// <summary>
        /// URL used to download bundle updates
        /// </summary>
        public static string BundleURL { get; set; }

        /// <summary>
        /// Language currently selected for localization
        /// </summary>
        public static string CurrentLanguage
        {
            get
            {
                if (!Initialized) Initialize();
                return currentLanguage;
            }
            set
            {
                if (!Initialized) Initialize();

                if (!Languages.Contains(value))
                {
                    StringExtensions.LogWarning("Localize.cs", "CurrentLanguage", "Unknown language '" + value + "' requested");
                    return;
                }

                currentLanguage = value;
                GetCurrentLanguage(value);
                PlayerPrefs.SetString("language", value);
                PlayerPrefs.Save();

                if (LookupMode == DictionaryLookupMode.StoreInMemory)
                {
                    switch (SourceType)
                    {
                        case DictionarySource.AssetBundle:
                            ResetFromAssetBundle();
                            break;
                        case DictionarySource.ResourceFile:
                            ResetFromResourceFile();
                            break;
                    }
                }

                onLanguageChanged?.Invoke(currentLanguage);
            }
        }

        /// <summary>
        /// Language to use by default
        /// </summary>
        public static string DefaultLanguage { get; private set; }

        /// <summary>
        /// Text encoding used in localization data
        /// </summary>
        public static TextEncoding Encoding { get; set; }

        /// <summary>
        /// Relative path to resource file
        /// </summary>
        public static string Filename { get; set; }

        /// <summary>
        /// Returns the initialized state of the system
        /// </summary>
        public static bool Initialized { get; private set; }

        /// <summary>
        /// Returns a list of languages known to the system
        /// </summary>
        public static List<string> Languages { get; private set; }

        /// <summary>
        /// Determines how lookup data is accessed
        /// </summary>
        public static DictionaryLookupMode LookupMode
        {
            get { return lookupMode; }
            set
            {
                if (lookupMode == value) return;
                lookupMode = value;
                onSettingsChanged?.Invoke();
            }
        }

        /// <summary>
        /// Source type of localization data
        /// </summary>
        public static DictionarySource SourceType
        {
            get { return sourceType; }
            set
            {
                if (sourceType == value) return;
                sourceType = value;
                onSettingsChanged?.Invoke();
            }
        }

        #endregion

        #region Pubic Methods

        /// <summary>
        /// Initialize localization
        /// </summary>
        public static void Initialize()
        {
            Initialize(InterfaceManager.localizationSettings);
        }

        /// <summary>
        /// Initialize localization
        /// </summary>
        /// <param name="settings"></param>
        public static void Initialize(LocalizationSettings settings)
        {
            if (initializing || Initialized) return;
            initializing = true;

            if (settings != null)
            {
                // Apply settings
                sourceType = settings.sourceType;
                lookupMode = settings.lookupMode;
                Encoding = settings.encoding;
                DefaultLanguage = settings.defaultLanguage;
                BundleAssetName = settings.bundleAssetName;
                BundleName = settings.bundleName;
                BundleURL = settings.bundleUrl;
                Filename = settings.filename;
            }
            else
            {
                initializing = false;
                Initialized = true;
                return;
            }

            // Initialize Data
            switch (SourceType)
            {
                case DictionarySource.AssetBundle:
                    InitializeFromAssetBundle();
                    break;
                case DictionarySource.ResourceFile:
                    InitializeFromResourceFile();
                    break;
            }

            initializing = false;
            Initialized = true;
        }

        /// <summary>
        /// Returns a localized string replacing [entry:ID] with localized entries
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GetFormattedString(string format)
        {
            if (string.IsNullOrEmpty(format)) return format;

            string entry;
            int i = format.IndexOf("[entry:", System.StringComparison.OrdinalIgnoreCase);
            int e;

            while (i >= 0)
            {
                e = format.IndexOf(']', i + 1);
                if (e < 0) e = format.Length;

                entry = format.Substring(i, e - i + 1);
                format = format.Replace(entry, GetString(entry.Substring(7, entry.Length - 8)));

                i = format.IndexOf("[entry:", System.StringComparison.OrdinalIgnoreCase);
            }

            return format;
        }

        /// <summary>
        /// Returns localized string from a specific entry key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(string key)
        {
            if (!Initialized && !initializing)
            {
                Initialize();
            }

            switch (LookupMode)
            {
                case DictionaryLookupMode.ReadOnRequest:
                    if (SourceType == DictionarySource.AssetBundle)
                    {
                        return RealtimeFromAssetBundle(key).Replace("<br/>", "\r\n").Replace("&lt;", "<").Replace("&gt;", ">");
                    }
                    return RealtimeFromResourceFile(key);
                default:
                    if (!dictionary.ContainsKey(key)) return string.Empty;
                    return dictionary[key];
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load dictionary data from asset bundle
        /// </summary>
        private static void InitializeFromAssetBundle()
        {
            AssetBundle bundle = LoadBundle(BundleName);
            if (bundle == null)
            {
                StringExtensions.LogWarning("Localize.cs", "CurrentLanguage", "Localize could not load asset bundle '" + BundleName + "'");
                return;
            }

            TextAsset textAsset = bundle.LoadAsset<TextAsset>(BundleAssetName);
            if (textAsset == null)
            {
                StringExtensions.LogWarning("Localize.cs", "CurrentLanguage", "Localize could not load bundled asset '" + BundleAssetName + "'");
                return;
            }

            LoadTextAsset(textAsset);

            // Raise language changed event
            onLanguageChanged?.Invoke(currentLanguage);
        }

        /// <summary>
        /// Load dictionary data from resource file
        /// </summary>
        private static void InitializeFromResourceFile()
        {
            // Get asset
            TextAsset textAsset = Resources.Load(Filename) as TextAsset;
            if (textAsset == null)
            {
                StringExtensions.LogWarning("Localize.cs", "CurrentLanguage", "Localize could not load asset '" + Filename + "'");
                return;
            }

            LoadTextAsset(textAsset);

            // Raise language changed event
            onLanguageChanged?.Invoke(currentLanguage);
        }

        /// <summary>
        /// Find the correct language to use and set the index
        /// </summary>
        private static void GetCurrentLanguage(string requestedLangauge)
        {
            if (string.IsNullOrEmpty(requestedLangauge))
            {
                requestedLangauge = DefaultLanguage;
            }

            for (int i = 0; i < Languages.Count; i++)
            {
                if (Languages[i] == requestedLangauge)
                {
                    currentLanguage = requestedLangauge;
                    lngIndex = i + 1;
                    return;
                }
            }

            currentLanguage = Languages[0];
            lngIndex = 1;
        }

        private static AssetBundle LoadBundle(string bundleName)
        {
            foreach (var bundle in AssetBundle.GetAllLoadedAssetBundles())
            {
                if (bundle.name == bundleName)
                {
                    return bundle;
                }
            }

            return AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleName));
        }

        private static void LoadDictionary(CsvParser.CsvReader csv)
        {
            string[] columns;
            dictionary = new Dictionary<string, string>();
            while (csv.MoveNext())
            {
                columns = csv.Current.ToArray();
                if (columns.Length > lngIndex && !string.IsNullOrEmpty(columns[lngIndex]))
                {
                    dictionary.Add(columns[0], columns[lngIndex].Replace("<br/>", "\r\n").Replace("&lt;", "<").Replace("&gt;", ">"));
                }
            }
        }

        private static void LoadTextAsset(TextAsset textAsset)
        {
            // Load data
            string[] columns;
            using (MemoryStream ms = new MemoryStream(textAsset.bytes))
            {
                using (CsvParser.CsvReader csv = new CsvParser.CsvReader(ms, Encoding == TextEncoding.UTF8 ? System.Text.Encoding.UTF8 : System.Text.Encoding.UTF32))
                {
                    // Read header for languages
                    Languages = new List<string>();
                    csv.MoveNext();
                    columns = csv.Current.ToArray();
                    for (int i = 1; i < columns.Length; i++)
                    {
                        Languages.Add(columns[i]);
                    }

                    // Set default language
                    DefaultLanguage = Languages[0];

                    // Set current langage
                    GetCurrentLanguage(PlayerPrefs.GetString("language"));

                    switch (LookupMode)
                    {
                        case DictionaryLookupMode.ReadOnRequest:
                            dictionary = null;
                            break;
                        case DictionaryLookupMode.StoreInMemory:
                            LoadDictionary(csv);
                            break;
                    }
                }
            }
        }

        private static string LoadTextAssetRealtime(TextAsset textAsset, string key)
        {
            // Load data
            string[] columns;
            using (MemoryStream ms = new MemoryStream(textAsset.bytes))
            {
                using (CsvParser.CsvReader csv = new CsvParser.CsvReader(ms, Encoding == TextEncoding.UTF8 ? System.Text.Encoding.UTF8 : System.Text.Encoding.UTF32))
                {
                    csv.MoveNext();
                    while (csv.MoveNext())
                    {
                        columns = csv.Current.ToArray();
                        if (columns[0] == key)
                        {
                            return columns[lngIndex];
                        }
                    }
                }
            }

            return string.Empty;
        }

        private static string RealtimeFromAssetBundle(string key)
        {
            AssetBundle bundle = LoadBundle(BundleName);
            if (bundle == null)
            {
                StringExtensions.LogError("Localize.cs", "CurrentLanguage", "Localize could not load asset bundle '" + BundleName + "'");
                return string.Empty;
            }

            TextAsset textAsset = bundle.LoadAsset<TextAsset>(BundleAssetName);
            if (textAsset == null)
            {
                StringExtensions.LogError("Localize.cs", "CurrentLanguage", "Localize could not load bundled asset '" + BundleAssetName + "'");
                return string.Empty;
            }

            return LoadTextAssetRealtime(textAsset, key);
        }

        private static string RealtimeFromResourceFile(string key)
        {
            // Get asset
            TextAsset textAsset = Resources.Load(Filename) as TextAsset;
            if (textAsset == null)
            {
                StringExtensions.LogError("Localize.cs", "CurrentLanguage", "Localize could not load asset '" + Filename + "'");
                return string.Empty;
            }

            return LoadTextAssetRealtime(textAsset, key);
        }

        private static void ResetFromAssetBundle()
        {
            AssetBundle bundle = LoadBundle(BundleName);
            if (bundle == null)
            {
                StringExtensions.LogError("Localize.cs", "CurrentLanguage", "Localize could not load asset bundle '" + BundleName + "'");
                return;
            }

            TextAsset textAsset = bundle.LoadAsset<TextAsset>(BundleAssetName);
            if (textAsset == null)
            {
                StringExtensions.LogError("Localize.cs", "CurrentLanguage", "Localize could not load bundled asset '" + BundleAssetName + "'");
                return;
            }

            // Load data
            using (MemoryStream ms = new MemoryStream(textAsset.bytes))
            {
                using (CsvParser.CsvReader csv = new CsvParser.CsvReader(ms, Encoding == TextEncoding.UTF8 ? System.Text.Encoding.UTF8 : System.Text.Encoding.UTF32))
                {
                    csv.MoveNext();
                    dictionary = new Dictionary<string, string>();
                    LoadDictionary(csv);
                }
            }
        }

        private static void ResetFromResourceFile()
        {
            // Get asset
            TextAsset textAsset = Resources.Load(Filename) as TextAsset;
            if (textAsset == null)
            {
                StringExtensions.LogError("Localize.cs", "CurrentLanguage", "Localize could not load asset '" + Filename + "'");
                return;
            }

            // Load data
            using (MemoryStream ms = new MemoryStream(textAsset.bytes))
            {
                using (CsvParser.CsvReader csv = new CsvParser.CsvReader(ms, Encoding == TextEncoding.UTF8 ? System.Text.Encoding.UTF8 : System.Text.Encoding.UTF32))
                {
                    csv.MoveNext();
                    dictionary = new Dictionary<string, string>();
                    LoadDictionary(csv);
                }
            }
        }

        #endregion

    }
}