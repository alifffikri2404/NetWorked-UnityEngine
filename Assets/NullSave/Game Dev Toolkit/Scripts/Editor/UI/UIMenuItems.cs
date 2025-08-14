using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    public class UIMenuItems
    {

        #region Menu Methods

        [MenuItem("GameObject/UI/GDTK/Button", false)]
        private static void AddButton()
        {
            var window = Resources.Load<GameObject>("UI/Prefabs/GDTK Button");
            Selection.activeObject = GameObject.Instantiate(window, GetTargetCanvas());
            Selection.activeObject.name = "GDTK Button";
        }

        [MenuItem("GameObject/UI/GDTK/Checkbox", false)]
        private static void AddCheckbox()
        {
            var window = Resources.Load<GameObject>("UI/Prefabs/GDTK Checkbox");
            Selection.activeObject = GameObject.Instantiate(window, GetTargetCanvas());
            Selection.activeObject.name = "GDTK Checkbox";
        }

        [MenuItem("GameObject/UI/GDTK/FlexList", false)]
        private static void AddFlexList()
        {
            var window = Resources.Load<GameObject>("UI/Prefabs/GDTK FlexList");
            Selection.activeObject = GameObject.Instantiate(window, GetTargetCanvas());
            Selection.activeObject.name = "GDTK FlexList";
        }

        [MenuItem("GameObject/UI/GDTK/Inline List", false)]
        private static void AddInlineList()
        {
            var window = Resources.Load<GameObject>("UI/Prefabs/GDTK Inline List");
            Selection.activeObject = GameObject.Instantiate(window, GetTargetCanvas());
            Selection.activeObject.name = "GDTK Inline List";
        }

        [MenuItem("GameObject/UI/GDTK/Label", false)]
        private static void AddLabel()
        {
            var window = Resources.Load<GameObject>("UI/Prefabs/GDTK Label");
            Selection.activeObject = GameObject.Instantiate(window, GetTargetCanvas());
            Selection.activeObject.name = "GDTK Label";
        }

        [MenuItem("GameObject/3D Object/GDTK/Label (TMP Text)", false)]
        private static void AddLabelTMPText()
        {
            GameObject go = new GameObject("Label (TMP Text)");

            TMP_Text tmpro = go.AddComponent<TextMeshPro>();
            tmpro.text = "Label Text";
            tmpro.alignment = TextAlignmentOptions.Left;
            tmpro.verticalAlignment = VerticalAlignmentOptions.Top;
            tmpro.fontSizeMin = 10;
            tmpro.enableAutoSizing = false;
            tmpro.color = Color.white;
            tmpro.fontSize = 18;

            UILabel_TMPText label = go.AddComponent<UILabel_TMPText>();
            label.text = "Label Text";

            UnityEditorInternal.ComponentUtility.MoveComponentUp(label);
            UnityEditorInternal.ComponentUtility.MoveComponentUp(label);

            Selection.activeObject = go;
        }

        [UnityEditor.MenuItem("GameObject/UI/GDTK/Progressbar", false)]
        private static void AddProgressbar()
        {
            var window = Resources.Load<GameObject>("UI/Prefabs/GDTK Progressbar");
            Selection.activeObject = GameObject.Instantiate(window, GetTargetCanvas());
            Selection.activeObject.name = "GDTK Progressbar";
        }

        [UnityEditor.MenuItem("GameObject/UI/GDTK/UI Menu", false)]
        private static void AddUIMenu()
        {
            var window = Resources.Load<GameObject>("UI/Prefabs/GDTK UI Menu");
            Selection.activeObject = GameObject.Instantiate(window, GetTargetCanvas());
            Selection.activeObject.name = "GDTK UI Menu";
        }

        #endregion

        #region Private Methods

        private static Transform GetTargetCanvas()
        {
            Canvas c = null;
            if(Selection.activeGameObject != null)
            {
                c = Selection.activeGameObject.GetComponent<Canvas>();
                if(c == null)
                {
                    RectTransform rt = Selection.activeGameObject.GetComponent<RectTransform>();
                    if (rt != null) return rt.transform;
                }
            }

            if(c == null)
            {
                c = GameObject.FindObjectOfType<Canvas>();
            }

            if(c == null)
            {
                return null;
                //GameObject go = new GameObject("Canvas");
                //c = go.AddComponent<Canvas>();
                //go.AddComponent<CanvasScaler>();
                //go.AddComponent<GraphicRaycaster>();
            }

            return c.transform;
        }

        #endregion

    }
}