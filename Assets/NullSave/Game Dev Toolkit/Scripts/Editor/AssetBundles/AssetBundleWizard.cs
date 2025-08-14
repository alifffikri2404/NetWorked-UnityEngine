using System.IO;
using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    public class AssetBundleWizard : EditorWindow
    {

        #region Constants

        private const string TITLE = "Asset Bundle Wizard";

        #endregion

        #region Fields

        private Vector2 windowScroll, panelScroll;
        private TextAsset activeTextAsset;

        public string bundleOutput = "Assets/AssetBundles";
        public BuildAssetBundleOptions bundleOptions;
        public BuildTarget buildTarget;

        #endregion

        #region Unity Methods

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.Space(4);
            windowScroll = GUILayout.BeginScrollView(windowScroll);

            bundleOutput = EditorGUILayout.TextField("Bundle Output", bundleOutput);
            bundleOptions = (BuildAssetBundleOptions)EditorGUILayout.EnumFlagsField("Options", bundleOptions);
            buildTarget = (BuildTarget)EditorGUILayout.EnumPopup("Target", buildTarget);

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Build Asset Bundle"))
            {
                if (!Directory.Exists(bundleOutput))
                {
                    Directory.CreateDirectory(bundleOutput);
                }

                PlayerPrefs.SetString("lcBundleOutput", bundleOutput);
                PlayerPrefs.SetInt("lcBundleOptions", (int)bundleOptions);
                PlayerPrefs.SetInt("lcBuildTarget", (int)buildTarget);

                BuildPipeline.BuildAssetBundles(bundleOutput, bundleOptions, buildTarget);
                this.Close();
            }

            GUILayout.EndScrollView();
            GUILayout.Space(4);
            GUILayout.EndVertical();
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        #endregion

        #region Public Methods

        [MenuItem("Tools/GDTK/Asset Bundle Wizard", false, -100)]
        public static void ShowWindow()
        {
            AssetBundleWizard w = GetWindow<AssetBundleWizard>(true, TITLE);

            w.titleContent = new GUIContent(TITLE);
            w.maxSize = w.minSize = new Vector2(400, 110);
            w.bundleOutput = PlayerPrefs.GetString("lcBundleOutput", "Assets/AssetBundles");
            w.bundleOptions = (BuildAssetBundleOptions)PlayerPrefs.GetInt("lcBundleOptions", 32);
            w.buildTarget = (BuildTarget)PlayerPrefs.GetInt("lcBuildTarget", 5);

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

        #endregion

    }
}