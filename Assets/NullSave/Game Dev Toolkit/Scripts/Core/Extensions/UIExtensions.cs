using UnityEngine;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    public static class UIExtensions
    {

        /// <summary>
        /// Ensure an item is visible within a scroll rect by adjust scrollbar positions as needed
        /// </summary>
        /// <param name="scrollRect"></param>
        /// <param name="target"></param>
        public static void EnsureItemVisibility(this ScrollRect scrollRect, GameObject target)
        {
            if (scrollRect == null || target == null) return;

            RectTransform rt = target.GetComponent<RectTransform>();
            if (rt == null) return;

            // Vertical Visibility
            if (scrollRect.verticalScrollbar != null && scrollRect.verticalScrollbar.gameObject.activeSelf)
            {
                float viewportHeight = scrollRect.viewport.rect.height;
                float overflowHeight = scrollRect.content.rect.height - viewportHeight;

                float top = overflowHeight * (1 - scrollRect.verticalScrollbar.value);
                float bottom = top + scrollRect.viewport.rect.height;

                float y1 = Mathf.Abs(rt.offsetMax.y);
                float y2 = y1 + rt.sizeDelta.y;

                if (y2 >= bottom)
                {
                    scrollRect.verticalScrollbar.value = 1 - ((y2 - viewportHeight) / overflowHeight);
                }

                if (y1 < top)
                {
                    scrollRect.verticalScrollbar.value = 1 - (y1 / overflowHeight);
                }
            }

            // Horizontal Visibility
            if (scrollRect.horizontalScrollbar != null && scrollRect.horizontalScrollbar.gameObject.activeSelf)
            {
                float viewportWidth = scrollRect.viewport.rect.width;
                float overflowWidth = scrollRect.content.rect.width - viewportWidth;

                float top = overflowWidth * (1 - scrollRect.horizontalScrollbar.value);
                float bottom = top + scrollRect.viewport.rect.width;

                float x1 = Mathf.Abs(rt.offsetMax.x);
                float x2 = x1 + rt.sizeDelta.x;

                if (x2 >= bottom)
                {
                    scrollRect.horizontalScrollbar.value = 1 - ((x2 - viewportWidth) / overflowWidth);
                }

                if (x1 < top)
                {
                    scrollRect.horizontalScrollbar.value = 1 - (x1 / overflowWidth);
                }
            }
        }

    }
}