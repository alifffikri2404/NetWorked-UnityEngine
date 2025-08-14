using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    [Serializable]
    public class ToggleEvent : UnityEvent<bool> { }

    [AutoDoc("The Checkbox component is a Selectable that controls a graphic which displays the on/off state and a Label of localizable text.")]
    [ExecuteInEditMode]
    public class Checkbox : Selectable, IPointerClickHandler, ISubmitHandler, ICanvasElement
    {

        #region Enumerations

        public enum ToggleTransition
        {
            None,
            Fade
        }

        #endregion

        #region Fields

        [Tooltip("Transition mode for the toggle.")] public ToggleTransition toggleTransition = ToggleTransition.Fade;
        [Tooltip("Graphic affected by the toggle.")] public Graphic graphic;

        [Tooltip("Add Checkbox to a Toggle Group")] public bool useToggleGroup;
        [Tooltip("Name of the Toggle Group to use")] public string groupName;

        [SerializeField]
        [Tooltip("Controls the on/off state")]
        private bool m_IsOn;

        [SerializeField, TextArea(2, 6)][Tooltip("Text to display with toggle")] private string m_Text;
        [Tooltip("Color of text when interactable.")] public Color enabledTextColor;
        [Tooltip("Color of text when not interactable.")] public Color disabledTextColor;
        [Tooltip("Automatically localize text.")] public bool localize;

        [Tooltip("Event raised when text changes.")] public UnityEvent onTextChanged;
        [Tooltip("Event raised when on/off state changes.")] public ToggleEvent onValueChanged = new ToggleEvent();
        [Tooltip("Event raised when state becomes on.")] public UnityEvent onChecked;
        [Tooltip("Event raised when state becomes off.")] public UnityEvent onUnchecked;

        [Tooltip("Component used to display text.")] public TextMeshProUGUI target;

        #endregion

        #region Properties

        [AutoDoc("Gets/Sets the on/off state.", "sampleObject.isOn = true;")]
        public bool isOn
        {
            get { return m_IsOn; }
            set
            {
                Set(value);
            }
        }

        [AutoDoc("Gets/Sets text. On set text is localized if the *localize* flag is set", "sampleObject.text = \"Hello world\";")]
        public string text
        {
            get { return target.text; }
            set
            {
                if (m_Text == value) return;
                m_Text = value;

                if (localize)
                {
                    target.text = Localize.GetFormattedString(value);
                    onTextChanged?.Invoke();
                }
                else
                {
                    target.text = m_Text;
                    onTextChanged?.Invoke();
                }
            }
        }

        [AutoDoc("Returns the TextMeshPro object displaying text", "sampleObject.textMeshPro.SetLayoutDirty();")]
        public TextMeshProUGUI textMeshPro
        {
            get { return target; }
        }

        [AutoDoc("Returns the text supplied before localization", "string originalText = sampleObject.unlocalizedText;")]
        public string unlocalizedText
        {
            get { return m_Text; }
        }

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();

            //target = GetComponentInChildren<TextMeshProUGUI>();
            ToolRegistry.RegisterComponent(this);

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

#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        protected void Reset()
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        {
            target = GetComponentInChildren<TextMeshProUGUI>();
            enabledTextColor = Color.black;
            disabledTextColor = colors.disabledColor;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            onValueChanged.Invoke(m_IsOn);
            if (m_IsOn)
            {
                onChecked?.Invoke();
            }
            else
            {
                onUnchecked?.Invoke();
            }
            PlayEffect(true);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            InternalToggle();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            InternalToggle();
        }

        public void Rebuild(CanvasUpdate executing)
        {
#if UNITY_EDITOR
            if (executing == CanvasUpdate.Prelayout)
            {
                onValueChanged.Invoke(m_IsOn);
            }
#endif
        }

        public void LayoutComplete() { }

        public void GraphicUpdateComplete() { }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            Set(m_IsOn, false);
            PlayEffect(toggleTransition == ToggleTransition.None);
        }

#endif

        #endregion

        #region Private Methods

        private void InternalToggle()
        {
            if (!IsActive() || !IsInteractable())
                return;

            if (useToggleGroup && isOn) return;

            isOn = !isOn;
        }

        private void LanguageChanged(string newLanguage)
        {
            if (!localize || target == null) return;

            target.text = Localize.GetFormattedString(m_Text);
        }

        private void PlayEffect(bool instant)
        {
            if (graphic == null) return;

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                graphic.canvasRenderer.SetAlpha(m_IsOn ? 1f : 0f);
            }
            else
#endif
                graphic.CrossFadeAlpha(m_IsOn ? 1f : 0f, instant ? 0f : 0.1f, true);
        }

        private void Set(bool value)
        {
            Set(value, true);
        }

        private void Set(bool value, bool sendCallback)
        {
            if (value && useToggleGroup)
            {
                if (Application.isPlaying)
                {
                    foreach (Checkbox chk in ToolRegistry.GetComponents<Checkbox>())
                    {
                        if (chk != this && chk.useToggleGroup && chk.groupName == groupName)
                        {
                            chk.isOn = false;
                        }
                    }
                }
                else
                {
                    foreach (Checkbox chk in FindObjectsOfType<Checkbox>())
                    {
                        if (chk != this && chk.useToggleGroup && chk.groupName == groupName)
                        {
                            chk.isOn = false;
                        }
                    }
                }
            }

            if (m_IsOn == value) return;

            // if we are in a group and set to true, do group logic
            m_IsOn = value;

            // Always send event when toggle is clicked, even if value didn't change
            // due to already active toggle in a toggle group being clicked.
            // Controls like SelectionList rely on this.
            // It's up to the user to ignore a selection being set to the same value it already was, if desired.
            PlayEffect(toggleTransition == ToggleTransition.None);
            if (sendCallback)
            {
                onValueChanged.Invoke(m_IsOn);
                if (m_IsOn)
                {
                    onChecked?.Invoke();
                }
                else
                {
                    onUnchecked?.Invoke();
                }
            }
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            if (target != null)
            {
                target.CrossFadeColor(interactable ? enabledTextColor : disabledTextColor, instant ? 0f : colors.fadeDuration, true, true);
            }
        }

        #endregion

    }
}