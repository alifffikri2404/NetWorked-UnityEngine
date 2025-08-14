using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UILabel_TMP : UILabel
    {

        #region Enumerations

        public enum AutoSizeMode
        {
            None,
            Vertically,
            Horizontally
        }

        #endregion

        #region Fields

        [SerializeField, TextArea(2, 6)] private string m_Text;
        public AutoSizeMode autoSize;
        public bool localize;

        public UnityEvent onResized;

        public TextMeshProUGUI target;
        private RectTransform RectTransform;

        #endregion

        #region Properties

        public override Color color
        {
            get { return target.color; }
            set { target.color = value; }
        }

        public override string text
        {
            get { return textMeshPro.text; }
            set
            {
                if (m_Text == value) return;
                m_Text = value;

                if (localize)
                {
                    textMeshPro.text = Localize.GetFormattedString(value);
                    onTextChanged?.Invoke();
                }
                else
                {
                    textMeshPro.text = m_Text;
                    onTextChanged?.Invoke();
                }

                UpdateSizing();
            }
        }

        public TextMeshProUGUI textMeshPro
        {
            get
            {
                if (target == null)
                {
                    target = GetComponent<TextMeshProUGUI>();
                }
                return target;
            }
        }

        public string unlocalizedText
        {
            get { return m_Text; }
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            Localize.onLanguageChanged.AddListener(LanguageChanged);
            if (Localize.Initialized)
            {
                LanguageChanged(Localize.CurrentLanguage);
            }
            else
            {
                Localize.Initialize();
            }
        }

        private void Start()
        {
            UpdateSizing();
        }

        private void Reset()
        {
            target = GetComponent<TextMeshProUGUI>();
        }

        #endregion

        #region Private Methods

        private void AutoSizeH()
        {
            if (RectTransform == null) RectTransform = GetComponent<RectTransform>();

            float width = Mathf.Max(RectTransform.rect.width, RectTransform.sizeDelta.x);
            if (width == 0)
            {
                StartCoroutine(DeferredAutoSize());
                return;
            }
            float height = Mathf.Max(RectTransform.rect.height, RectTransform.sizeDelta.y);

            Vector2 sizeTo = target.GetPreferredValues(float.PositiveInfinity, height);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sizeTo.x);

            onResized?.Invoke();
        }

        private void AutoSizeV()
        {
            if (RectTransform == null) RectTransform = GetComponent<RectTransform>();

            float width = Mathf.Max(RectTransform.rect.width, RectTransform.sizeDelta.x);
            if (width == 0)
            {
                StartCoroutine(DeferredAutoSize());
                return;
            }
            Vector2 sizeTo = target.GetPreferredValues(text, width, float.PositiveInfinity);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizeTo.y);

            onResized?.Invoke();
        }

        private IEnumerator DeferredAutoSize()
        {
            float width = 0;
            while (width == 0)
            {
                width = Mathf.Max(RectTransform.rect.width, RectTransform.sizeDelta.x);
                yield return null;
            }

            UpdateSizing();
        }

        private void LanguageChanged(string newLanguage)
        {
            if (!localize) return;

            textMeshPro.text = Localize.GetFormattedString(m_Text);
        }

        private void UpdateSizing()
        {
            switch (autoSize)
            {
                case AutoSizeMode.Horizontally:
                    AutoSizeH();
                    break;
                case AutoSizeMode.Vertically:
                    AutoSizeV();
                    break;

            }
        }

        #endregion

    }
}