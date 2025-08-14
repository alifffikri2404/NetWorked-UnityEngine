using UnityEngine;

namespace NullSave.GDTK
{
    public static class UIHelper
    {

        #region Public Methods

        public static void EnsureItemVisibleVertically(this RectTransform item, RectTransform container)
        {
            if (item.pivot.y != 1)
            {
                StringExtensions.LogError("UIHelper.cs", "EnsureItemVisibleVertically", "Item y pivot required to be be 1 not " + item.pivot.y);
                return;
            }

            if (container.anchoredPosition.y > -item.anchoredPosition.y)
            {
                container.anchoredPosition = new Vector2(container.anchoredPosition.x, -item.anchoredPosition.y);
                return;
            }

            RectTransform rtViewPort = container.transform.parent.GetComponent<RectTransform>();
            float itemBottom = -item.anchoredPosition.y + item.sizeDelta.y;
            float containerBottom = rtViewPort.rect.height + container.anchoredPosition.y;

            if (itemBottom > containerBottom)
            {
                container.anchoredPosition += new Vector2(0, itemBottom - containerBottom);
            }
        }

        public static Vector3 GetPointOnRectEdge(RectTransform rect, Vector2 dir)
        {
            if (rect == null) return Vector3.zero;

            if (dir != Vector2.zero)
            {
                dir /= Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
            }
            dir = rect.rect.center + Vector2.Scale(rect.rect.size, dir * 0.5f);

            return dir;
        }

        #endregion

    }
}