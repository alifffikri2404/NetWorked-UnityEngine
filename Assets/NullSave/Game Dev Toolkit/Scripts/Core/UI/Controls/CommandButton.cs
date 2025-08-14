using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    [AutoDoc("A standard button that can be clicked in order to trigger an event with support for text coloring and localization.")]
    public class CommandButton : Selectable, IPointerClickHandler, ICanvasElement, ISelectHandler, IDeselectHandler, ISubmitHandler
    {

        #region Fields

        [Tooltip("Control displaying text")] public TextMeshProUGUI target;
        [SerializeField, TextArea(2, 6)] [Tooltip("Button text")] private string m_text;
        [Tooltip("Localize button text")] public bool localize;

        [Tooltip("Colorize tet")] public bool colorText;
        [Tooltip("Colors used on text component")][SerializeField]  private ColorBlock m_fontColors;

        [Tooltip("Event fired on button click")] public UnityEvent onClick;

        private CanvasRenderer cr;
        private bool lastMode;

        #endregion

        #region Properties

        [AutoDocSuppress]
        public Animator Animator
        {
            get { return GetComponent<Animator>(); }
        }

        [AutoDocSuppress]
        private CanvasRenderer CR
        {
            get
            {
                if (cr == null) cr = GetComponent<CanvasRenderer>();
                return cr;
            }
        }

        [AutoDoc("Colors used on text component", "sampleObject.fontColors.normalColor = Color.black;")]
        public ColorBlock fontColors
        {
            get { return m_fontColors; }
            set
            {
                if (m_fontColors.Equals(value)) return;
                m_fontColors = value;
                UpdateState();
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    UnityEditor.EditorUtility.SetDirty(this);
                }
#endif
            }
        }

        [AutoDoc("Check if pointer is over control", "Debug.Log(sampleObject.hasPointer);")]
        public bool hasPointer { get; private set; }

        [AutoDoc("Check if pointer is down on control", "Debug.Log(sampleObject.pointerDown);")]
        public bool pointerDown { get; private set; }

        [AutoDoc("Check if control is currently selected", "Debug.Log(sampleObject.selected);")]
        public bool selected { get; private set; }

        [AutoDoc("Get/Set control text. If localize is selected text will automatically be localized.", "sampleObject.text = \"Button Text\";")]
        public string text
        {
            get => target != null ? target.text : null;
            set
            {
                if (m_text == value) return;
                m_text = value;

                if (target.text == null) return;

                if (localize)
                {
                    target.text = Localize.GetFormattedString(value);
                }
                else
                {
                    target.text = m_text;
                }
            }
        }

        #endregion

        #region Unity Methods

        [AutoDocSuppress]
        protected override void Awake()
        {
            base.Awake();

            lastMode = interactable;
            target = GetComponentInChildren<TextMeshProUGUI>();

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
        [AutoDocSuppress]
        protected void Reset()
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        {
            target = GetComponentInChildren<TextMeshProUGUI>();

            // Text
            m_fontColors = new ColorBlock();
            m_fontColors.colorMultiplier = 1;
            m_fontColors.normalColor = Color.black;
            m_fontColors.selectedColor = Color.black;
            m_fontColors.highlightedColor = Color.black;
            m_fontColors.pressedColor = Color.black;
            m_fontColors.disabledColor = new Color(0, 0, 0, 0.5019608f);
            m_fontColors.fadeDuration = 0.1f;

            colorText = true;
        }

        [AutoDocSuppress]
        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            selected = false;
            UpdateState();
        }

        [AutoDocSuppress]
        protected override void OnEnable()
        {
            base.OnEnable();
            if (m_fontColors == null) m_fontColors = new ColorBlock();
            UpdateState();
        }

        [AutoDocSuppress]
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;
            if (!interactable) return;

            UpdateState();
            onClick?.Invoke();
        }

        [AutoDocSuppress]
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            pointerDown = true;
            UpdateState();
        }

        [AutoDocSuppress]
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            hasPointer = true;
            UpdateState();
        }

        [AutoDocSuppress]
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            hasPointer = false;
            UpdateState();
        }

        [AutoDocSuppress]
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            pointerDown = false;
            UpdateState();
        }

        [AutoDocSuppress]
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            selected = true;
            UpdateState();
        }

        [AutoDocSuppress]
        public void OnSubmit(BaseEventData eventData)
        {
            onClick?.Invoke();
        }

        [AutoDocSuppress]
        public void Rebuild(CanvasUpdate executing)
        {
#if UNITY_EDITOR
            if (executing == CanvasUpdate.Prelayout)
            {
                UpdateState();
            }
#endif
        }

        [AutoDocSuppress]
        public void LayoutComplete() { }

        [AutoDocSuppress]
        public void GraphicUpdateComplete() { }

#if UNITY_EDITOR
        [AutoDocSuppress]
        protected override void OnValidate()
        {
            base.OnValidate();
            UpdateState();
        }

#endif

        private void Update()
        {
            if(lastMode != interactable)
            {
                UpdateState();
                lastMode = interactable;
            }
        }

        #endregion

        #region Private Methods

        [AutoDocSuppress]
        protected virtual void DoStateTransition(UISelectionState state, bool instant)
        {
            if (colors == null) return;

            Color tintColor, fontColor;
            Sprite transitionSprite;
            string triggerName;

            switch (state)
            {
                case UISelectionState.Normal:
                    tintColor = colors.normalColor;
                    fontColor = fontColors.normalColor;
                    transitionSprite = null;
                    triggerName = animationTriggers.normalTrigger;
                    break;
                case UISelectionState.Highlighted:
                    tintColor = colors.highlightedColor;
                    fontColor = fontColors.highlightedColor;
                    transitionSprite = spriteState.highlightedSprite;
                    triggerName = animationTriggers.highlightedTrigger;
                    break;
                case UISelectionState.Pressed:
                    tintColor = colors.pressedColor;
                    fontColor = fontColors.pressedColor;
                    transitionSprite = spriteState.pressedSprite;
                    triggerName = animationTriggers.pressedTrigger;
                    break;
                case UISelectionState.Disabled:
                    tintColor = colors.disabledColor;
                    fontColor = fontColors.disabledColor;
                    transitionSprite = spriteState.disabledSprite;
                    triggerName = animationTriggers.disabledTrigger;
                    break;
                case UISelectionState.Selected:
                    tintColor = colors.selectedColor;
                    fontColor = fontColors.selectedColor;
                    transitionSprite = spriteState.selectedSprite;
                    triggerName = animationTriggers.selectedTrigger;
                    break;
                default:
                    tintColor = Color.black;
                    fontColor = Color.white;
                    transitionSprite = null;
                    triggerName = string.Empty;
                    break;
            }

            if (gameObject.activeInHierarchy)
            {
                switch (transition)
                {
                    case Transition.ColorTint:
                        StartColorTween(tintColor * colors.colorMultiplier, instant);
                        break;
                    case Transition.SpriteSwap:
                        DoSpriteSwap(transitionSprite);
                        CR.SetColor(Color.white);
                        break;
                    case Transition.Animation:
                        TriggerAnimation(triggerName);
                        CR.SetColor(Color.white);
                        break;
                    default:
                        CR.SetColor(Color.white);
                        break;
                }

                if (target != null && colorText)
                {
                    target.CrossFadeColor(fontColor, instant ? 0f : fontColors.fadeDuration, true, true);
                }
            }
        }

        private void DoSpriteSwap(Sprite newSprite)
        {
            if (image == null) return;
            image.overrideSprite = newSprite;
        }

        private void LanguageChanged(string newLanguage)
        {
            if (!localize) return;

            target.text = Localize.GetFormattedString(m_text);
        }

        private void StartColorTween(Color targetColor, bool instant)
        {
            if (targetGraphic == null) return;
            targetGraphic.CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
        }

        private void TriggerAnimation(string triggername)
        {
            if (Animator == null || !Animator.enabled || !Animator.isActiveAndEnabled || Animator.runtimeAnimatorController == null || string.IsNullOrEmpty(triggername))
                return;

            Animator.ResetTrigger(animationTriggers.normalTrigger);
            Animator.ResetTrigger(animationTriggers.pressedTrigger);
            Animator.ResetTrigger(animationTriggers.highlightedTrigger);
            Animator.ResetTrigger(animationTriggers.disabledTrigger);
            Animator.SetTrigger(triggername);
        }

        private void UpdateState()
        {
            if (!interactable)
            {
                DoStateTransition(UISelectionState.Disabled, !Application.isPlaying);
            }
            else
            {
                DoStateTransition(pointerDown ? UISelectionState.Pressed : selected ? UISelectionState.Selected : hasPointer ? UISelectionState.Highlighted : UISelectionState.Normal, !Application.isPlaying);
            }
        }

        #endregion

    }
}