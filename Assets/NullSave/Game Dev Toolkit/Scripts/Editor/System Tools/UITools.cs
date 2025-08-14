using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    public class UITools
    {

        #region Public Methods

        public static GameObject AddAnchoredChild(string name, Transform parent, float minX, float minY, float maxX, float maxY, float left = 0, float top = 0, float right = 0, float bottom = 0)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent);
            go.transform.localScale = new Vector3(1, 1, 1);

            RectTransform rt = go.AddComponent<RectTransform>();
            rt.anchorMax = new Vector2(maxX, maxY);
            rt.anchorMin = new Vector2(minX, minY);
            rt.offsetMin = new Vector2(left, bottom);
            rt.offsetMax = new Vector2(-right, -top);

            return go;
        }

        public static GameObject AddFullsizedChild(string name, Transform parent, float left = 0, float top = 0, float right = 0, float bottom = 0)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent);
            go.transform.localScale = new Vector3(1, 1, 1);

            RectTransform rt = go.AddComponent<RectTransform>();
            rt.anchorMax = new Vector2(1, 1);
            rt.anchorMin = Vector2.zero;
            rt.offsetMin = new Vector2(left, bottom);
            rt.offsetMax = new Vector2(-right, -top);

            return go;
        }

        public static GameObject AddLeftAlignedChild(string name, Transform parent, float width, float height, float left = 0, float right = 0)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent);
            go.transform.localScale = new Vector3(1, 1, 1);

            RectTransform rt = go.AddComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0, 0.5f);
            rt.anchoredPosition = new Vector2(left, right);
            rt.sizeDelta = new Vector2(width, height);

            return go;
        }

        public static GameObject AddLeftAlignedFullHeightChild(string name, Transform parent, float width)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent);
            go.transform.localScale = new Vector3(1, 1, 1);

            RectTransform rt = go.AddComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = new Vector2(0, 1);
            //rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0, 0.5f);
            rt.anchoredPosition = new Vector2(0, 0);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);

            return go;
        }

        public static GameObject AddRightAlignedChild(string name, Transform parent, float width, float height, float left = 0, float right = 0)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent);
            go.transform.localScale = new Vector3(1, 1, 1);

            RectTransform rt = go.AddComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(1, 0.5f);
            rt.anchoredPosition = new Vector2(left, right);
            rt.sizeDelta = new Vector2(width, height);

            return go;
        }

        public static Scrollbar AddScrollbar(GameObject go, Orientation orientation)
        {
            GameObject goScrollbar = new GameObject("Scrollbar " + orientation.ToString());
            goScrollbar.transform.SetParent(go.transform);
            goScrollbar.transform.localScale = new Vector3(1, 1, 1);

            RectTransform rtScrollbar = goScrollbar.AddComponent<RectTransform>();
            if(orientation == Orientation.Horizontal)
            {
                rtScrollbar.anchorMin = Vector2.zero;
                rtScrollbar.anchorMax = new Vector2(1, 0);
                rtScrollbar.pivot = Vector2.zero;
                rtScrollbar.offsetMax = rtScrollbar.offsetMin = Vector2.zero;
                rtScrollbar.sizeDelta += new Vector2(0, 20);
            }
            else
            {
                rtScrollbar.anchorMin = new Vector2(1, 0);
                rtScrollbar.anchorMax = new Vector2(1, 1);
                rtScrollbar.pivot = new Vector2(1, 1);
                rtScrollbar.offsetMax = rtScrollbar.offsetMin = Vector2.zero;
                rtScrollbar.sizeDelta += new Vector2(20, 0);
            }
            AddSlicedImage(goScrollbar, Resources.Load<Sprite>("flat-background-border"), new Color(0.5943396f, 0.5943396f, 0.5943396f));

            Scrollbar scrollbar = goScrollbar.AddComponent<Scrollbar>();

            GameObject goScrollArea = AddFullsizedChild("Slide Area", goScrollbar.transform);
            GameObject goHandle = AddFullsizedChild("Handle", goScrollArea.transform);
            goHandle.AddComponent<CanvasRenderer>();
            Image imgHandle = goHandle.AddComponent<Image>();
            imgHandle.sprite = Resources.Load<Sprite>("flat-background-border");
            imgHandle.type = Image.Type.Sliced;

            scrollbar.handleRect = goHandle.GetComponent<RectTransform>();
            scrollbar.direction = orientation == Orientation.Horizontal ? Scrollbar.Direction.LeftToRight : Scrollbar.Direction.BottomToTop;
            scrollbar.targetGraphic = imgHandle;
         

            return scrollbar;
        }

        public static ScrollRect AddScrollView(GameObject go, bool horizontalScrollbar, bool verticalScrollbar)
        {
            ScrollRect sr = go.AddComponent<ScrollRect>();

            // Create viewport
            GameObject Viewport = AddFullsizedChild("Viewport", go.transform);
            Viewport.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            Viewport.AddComponent<CanvasRenderer>();
            Image imgVP = Viewport.AddComponent<Image>();
            imgVP.sprite = Resources.Load<Sprite>("flat-background-border");
            imgVP.maskable = true;
            imgVP.type = Image.Type.Sliced;
            Viewport.AddComponent<Mask>().showMaskGraphic = false;

            // Create Content
            GameObject Content = new GameObject("Content");
            Content.transform.SetParent(Viewport.transform);
            Content.transform.localScale = new Vector3(1, 1, 1);
            RectTransform rtContent = Content.AddComponent<RectTransform>();
            rtContent.anchorMin = new Vector2(0, 1);
            rtContent.anchorMax = new Vector2(1, 1);
            rtContent.pivot = new Vector2(0, 1);
            rtContent.offsetMax = rtContent.offsetMin = Vector2.zero;
            VerticalLayoutGroup vlg = Content.AddComponent<VerticalLayoutGroup>();
            vlg.padding = new RectOffset(1, 1, 1, 1);
            vlg.childControlHeight = false;
            vlg.childControlWidth = true;
            //vlg.childScaleHeight = vlg.childScaleWidth = false;
            vlg.childForceExpandHeight = false;
            vlg.childForceExpandWidth = true;

            // Assign data
            sr.viewport = Viewport.GetComponent<RectTransform>();
            sr.content = rtContent;
            if (horizontalScrollbar)
            {
                sr.horizontalScrollbar = AddScrollbar(go, Orientation.Horizontal);
                sr.horizontalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
                sr.horizontalScrollbarSpacing = -3;
            }
            if (verticalScrollbar)
            {
                sr.verticalScrollbar = AddScrollbar(go, Orientation.Vertical);
                sr.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
                sr.verticalScrollbarSpacing = -3;
            }

            sr.scrollSensitivity = 10;

            return sr;
        }

        public static Image AddSlicedImage(GameObject go, Sprite image)
        {
            return AddSlicedImage(go, image, Color.white);
        }

        public static Image AddSlicedImage(GameObject go, Sprite image, Color color)
        {
            Image img = go.AddComponent<Image>();
            img.sprite = image;
            img.type = Image.Type.Sliced;
            img.color = color;
            return img;
        }

        public static GameObject AddTopAlignedChild(string name, Transform parent, float height, float left = 0, float right = 0)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent);
            go.transform.localScale = new Vector3(1, 1, 1);

            RectTransform rt = go.AddComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(1, 1);
            rt.pivot = new Vector2(0, 1);
            rt.offsetMin = new Vector2(left, 0);
            rt.offsetMax = new Vector2(-right, 0);
            rt.sizeDelta += new Vector2(0, height);

            return go;
        }

        public static GameObject CreateAndParentUIControl(string name, float width, float height)
        {
            GameObject go = new GameObject(name);
            if (Selection.activeGameObject != null)
            {
                go.transform.parent = Selection.activeGameObject.transform;
                go.transform.localScale = new Vector3(1, 1, 1);
            }

            RectTransform rt = go.AddComponent<RectTransform>();
            rt.anchorMax = rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(width, height);
            rt.anchoredPosition = Vector2.zero;

            return go;
        }

        #endregion

    }
}