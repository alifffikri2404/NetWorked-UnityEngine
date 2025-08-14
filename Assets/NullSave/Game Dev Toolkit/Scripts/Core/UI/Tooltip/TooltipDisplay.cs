using TMPro;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDoc("This control provides the display component for tooltips.")]
    [RequireComponent(typeof(RectTransform))]
    public class TooltipDisplay : MonoBehaviour
    {

        #region Fields

        [Tooltip("Padding to add when resizing to fit tooltip text")] public Vector2 padding;
        [Tooltip("Maximum size of the tooltip")] public Vector2 maxSize;
        [Tooltip("Component used to display tooltip text")] public TextMeshProUGUI tipText;

        private RectTransform rt;

        #endregion

        #region Properties

        [AutoDocSuppress]
        public RectTransform RectTransform
        {
            get
            {
                if (rt == null) rt = GetComponent<RectTransform>();
                return rt;
            }
        }

        #endregion

        #region Unity Methods

        private void Reset()
        {
            padding = new Vector2(8, 8);
            maxSize = new Vector2(800, 200);
            tipText = GetComponentInChildren<TextMeshProUGUI>();
        }

        #endregion

        #region Public Methods

        [AutoDoc("Display tooltip text.", "sampleObject.ShowTip(\"Hello world\");")]
        [AutoDocParameter("Text to display")]
        public virtual void ShowTip(string text)
        {
            RectTransform.sizeDelta = maxSize;
            tipText.text = text;

            Vector2 sizeTo = tipText.GetPreferredValues(text, maxSize.x, maxSize.y);
            sizeTo += padding;
            RectTransform.sizeDelta = sizeTo;
        }

        #endregion

    }
}