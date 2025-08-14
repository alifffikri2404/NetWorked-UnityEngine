using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    [AutoDoc("Component used to display text that can automatically be localized.")]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class Label : UILabel
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

        [SerializeField, TextArea(2, 6)] [Tooltip("Text to display")] private string m_Text;
        [Tooltip("Auto-size mode")] public AutoSizeMode autoSize;
        [Tooltip("Localize text")] public bool localize;


#if ENABLE_INPUT_SYSTEM
        [Tooltip("Enable Action Icons for new Input System")] public bool useActionIcons;
        [Tooltip("Actions asset used for mapping actions")] public UnityEngine.InputSystem.InputActionAsset inputActions;
#endif

        [Tooltip("Event raised whenever control is resized")] public UnityEvent onResized;

        [Tooltip("Component used to display text.")] public TextMeshProUGUI target;

        private RectTransform RectTransform;

        #endregion

        #region Properties

        [AutoDoc("Color to apply to text.", "sampleObject.color = Color.black;")]
        public override Color color
        {
            get { return target.color; }
            set { target.color = value; }
        }

        [AutoDoc("Get/Set control text. If localize is selected text will automatically be localized.", "sampleObject.text = \"Button Text\";")]
        public override string text
        {
            get { return target.text; }
            set
            {
                if (m_Text == value) return;
                m_Text = value;

                if (localize)
                {
#if ENABLE_INPUT_SYSTEM
                    UpdateActionIcons();
#else
                    target.text = Localize.GetFormattedString(m_Text);
#endif
                    onTextChanged?.Invoke();
                }
                else
                {
#if ENABLE_INPUT_SYSTEM
                    UpdateActionIcons();
#else
                    target.text = m_Text;
#endif
                    onTextChanged?.Invoke();
                }

                UpdateSizing();
            }
        }

        [AutoDoc("Component used to display text.", "Debug.Log(sampleObject.textMeshPro.text);")]
        public TextMeshProUGUI textMeshPro
        {
            get { return target; }
        }

        [AutoDoc("Text supplied without localization.", "Debug.Log(sampleObject.unlocalizedText);")]
        public string unlocalizedText
        {
            get { return m_Text; }
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            target = GetComponent<TextMeshProUGUI>();
            Localize.onLanguageChanged.AddListener(LanguageChanged);
#if ENABLE_INPUT_SYSTEM
            if (useActionIcons)
            {
                ActionIcons.instance.onControllerChanged.AddListener(UpdateActionIcons);
            }
#endif
        }

        private void OnEnable()
        {
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

#if ENABLE_INPUT_SYSTEM
        private void UpdateActionIcons()
        {

            Dictionary<string, InputMap> result = new Dictionary<string, InputMap>();
            string format = localize ? Localize.GetFormattedString(m_Text) : m_Text;
            string actionName;
            int i, e;

            while (true)
            {
                i = format.IndexOf("{action:");
                if (i < 0) break;
                e = format.IndexOf("}", i + 1);
                if (e < 0) e = format.Length;

                actionName = format.Substring(i + 8, e - i - 8);
                if (!result.ContainsKey(actionName))
                {
                    result.Add(actionName, ActionIcons.GetActionIcon(inputActions, actionName));
                }

                if (e >= format.Length)
                {
                    e = format.Length - 1;
                }
                format = format.Substring(0, i) + format.Substring(e + 1);
            }

            format = localize ? Localize.GetFormattedString(m_Text) : m_Text;
            InputMap entryMap;
            foreach (var entry in result)
            {
                entryMap = entry.Value;
                if (entryMap != null) target.spriteAsset = entryMap.TMPSpriteAsset;
                format = format.Replace("{action:" + entry.Key + "}", entryMap == null ? "" : "<sprite index=" + entryMap.tmpSpriteIndex + " tint=1>");
            }

            target.text = format;
        }
#endif

        private void LanguageChanged(string newLanguage)
        {
#if ENABLE_INPUT_SYSTEM

            if (useActionIcons)
            {
                UpdateActionIcons();
                return;
            }
#endif
            if (!localize) return;

            target.text = Localize.GetFormattedString(m_Text);
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