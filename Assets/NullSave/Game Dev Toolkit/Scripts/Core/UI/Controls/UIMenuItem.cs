using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.Selectable;

namespace NullSave.GDTK
{
    [AutoDoc("This component provides a menu item for use with the UIMenu control.")]
    [ExecuteInEditMode]
    public class UIMenuItem : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, ISubmitHandler, IPointerEnterHandler, IPointerExitHandler
    {

        #region Members

        [SerializeField][Tooltip("Determine if control is interactable")] private bool m_interactable;

        [Tooltip("Background image for control")] public Image background;
        [Tooltip("The type of transition that will be applied to the targetGraphic when the state changes")] public Transition transition;
        [SerializeField] [Tooltip("The ColorBlock for this objects background")] private ColorBlock m_backgroundColors;

        [Tooltip("The label for displaying text on this control")] public Label textLabel;
        [Tooltip("Apply colors to text")] public bool colorText;
        [SerializeField] [Tooltip("The ColorBlock for this objects text")] private ColorBlock m_fontColors;
        [Tooltip("The SpriteState for this control")] public SpriteState spriteState;
        [Tooltip("The AnimationTriggers for this control")] public AnimationTriggers animationTriggers;

        [Tooltip("Event fired when this control is selected")] public UnityEvent onSelected;
        [Tooltip("Event fired when this control loses selection")] public UnityEvent onDeselected;
        [Tooltip("Event fired when this control is submitted")] public MenuItemEvent onSubmit;

        private CanvasRenderer cr;
        private bool m_selected;
        private bool immediate;

        #endregion

        #region Properties

        [AutoDocSuppress]
        public Animator animator { get; set; }

        [AutoDocSuppress]
        public ColorBlock backgroundColors
        {
            get { return m_backgroundColors; }
            set
            {
                if (m_backgroundColors.Equals(value)) return;
                m_backgroundColors = value;
                UpdateState();
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    UnityEditor.EditorUtility.SetDirty(this);
                }
#endif
            }
        }

        [AutoDocSuppress]
        private CanvasRenderer canvasRenderer
        {
            get
            {
                if (background == null) return null;
                if (cr == null) cr = background.GetComponent<CanvasRenderer>();
                return cr;
            }
        }

        [AutoDocSuppress]
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

        [AutoDoc("True if pointer is over control", "Debug.Log(sampleObject.hasPointer);")]
        public bool hasPointer { get; private set; }

        [AutoDoc("Determine if control is interactable", "sampleObject.interactable = true")]
        public bool interactable
        {
            get { return m_interactable; }
            set
            {
                if (m_interactable == value) return;
                m_interactable = value;
                UpdateState();
            }
        }

        [AutoDoc("Returns the UIMenu that owns this control", "sampleObject.parent.Close()")]
        public UIMenu parent { get; private set; }

        [AutoDoc("True if pointer is pressed over control", "Debug.Log(sampleObject.pointerDown);")]
        public bool pointerDown { get; private set; }

        [AutoDoc("True if control is selected", "Debug.Log(sampleObject.selected);")]
        public bool selected
        {
            get { return m_selected; }
            private set
            {
                if (m_selected == value) return;
                m_selected = value;
                if(m_selected)
                {
                    onSelected?.Invoke();
                }
                else
                {
                    onDeselected?.Invoke();
                }
            }
        }

        [AutoDoc("Gets/Sets the text on the control", "sampleObject.text = \"Hello World\";")]
        public string text
        {
            get => textLabel != null ? textLabel.text : null;
            set { if (textLabel != null) textLabel.text = value; }
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            animator = GetComponent<Animator>();
            parent = GetComponentInChildren<UIMenu>();

            Transform target = transform;
            while (target.parent != null && parent == null)
            {
                target = target.parent;
                parent = target.GetComponent<UIMenu>();
            }

            onDeselected?.Invoke();
        }

        private void OnEnable()
        {
            immediate = true;
            UpdateState();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (parent == null || !parent.menuNav.allowedInput.HasFlag(InputFlags.Click) || !interactable) return;
            parent.selectedItem = this;
            UpdateState();
            StartCoroutine(WaitAndSubmit());
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            pointerDown = true;
            UpdateState();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hasPointer = true;
            UpdateState();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hasPointer = false;
            UpdateState();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            pointerDown = false;
            UpdateState();
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
        }

        private void Reset()
        {
            textLabel = GetComponentInChildren<Label>();
            background = GetComponent<Image>();

            m_interactable = true;

            m_fontColors = new ColorBlock();
            m_fontColors.colorMultiplier = 1;
            m_fontColors.normalColor = Color.black;
            m_fontColors.selectedColor = Color.black;
            m_fontColors.highlightedColor = Color.black;
            m_fontColors.pressedColor = Color.black;
            m_fontColors.disabledColor = new Color(0, 0, 0, 0.5019608f);
            m_fontColors.fadeDuration = 0.5f;

            transition = Transition.ColorTint;
            m_backgroundColors = new ColorBlock();
            m_backgroundColors.colorMultiplier = 1;
            m_backgroundColors.normalColor = Color.white;
            m_backgroundColors.selectedColor = new Color(0.9607843f, 0.9607843f, 0.9607843f, 1);
            m_backgroundColors.highlightedColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 1);
            m_backgroundColors.pressedColor = new Color(0.9607843f, 0.9607843f, 0.9607843f, 1);
            m_backgroundColors.disabledColor = new Color(1, 1, 1, 0.5019608f);
            m_backgroundColors.fadeDuration = 0.5f;

            colorText = true;
        }

        #endregion

        #region Public Methods

        [AutoDoc("Force the control to refresh its UI", "sampleObject.Refresh();")]
        public void Refresh()
        {
            UpdateState();
        }

        #endregion

        #region Private Methods

        private string AnimationTriggerFromState(AnimationTriggers source, UISelectionState state)
        {
            return state switch
            {
                UISelectionState.Highlighted => source.highlightedTrigger,
                UISelectionState.Pressed => source.pressedTrigger,
                UISelectionState.Disabled => source.disabledTrigger,
                UISelectionState.Selected => source.selectedTrigger,
                _ => source.normalTrigger,
            };
        }

        private Color ColorFromState(ColorBlock source, UISelectionState state)
        {
            return state switch
            {
                UISelectionState.Normal => source.normalColor,
                UISelectionState.Highlighted => source.highlightedColor,
                UISelectionState.Pressed => source.pressedColor,
                UISelectionState.Disabled => source.disabledColor,
                UISelectionState.Selected => source.selectedColor,
                _ => Color.white,
            };
        }

        protected virtual void DoStateTransition(UISelectionState state, bool instant)
        {
            if (colorText)
            {
                TextStateTransition(state, instant);
            }

            if (background != null)
            {
                switch (transition)
                {
                    case Transition.Animation:
                        string triggerName = AnimationTriggerFromState(animationTriggers, state);
                        if (animator != null && animator.enabled && animator.isActiveAndEnabled && animator.runtimeAnimatorController != null && !string.IsNullOrEmpty(triggerName))
                        {
                            animator.ResetTrigger(animationTriggers.normalTrigger);
                            animator.ResetTrigger(animationTriggers.pressedTrigger);
                            animator.ResetTrigger(animationTriggers.highlightedTrigger);
                            animator.ResetTrigger(animationTriggers.disabledTrigger);
                            animator.SetTrigger(triggerName);
                        }
                        canvasRenderer.SetColor(Color.white);
                        break;
                    case Transition.ColorTint:
                        Color color = ColorFromState(backgroundColors, state);
                        background.CrossFadeColor(color, instant ? 0f : backgroundColors.fadeDuration, true, true);
                        break;
                    case Transition.SpriteSwap:
                        if(background != null)
                        {
                            background.sprite = SpriteFromState(spriteState, state);
                        }
                        canvasRenderer.SetColor(Color.white);
                        break;
                    default:
                        canvasRenderer.SetColor(Color.white);
                        break;
                }
            }
        }

        private Sprite SpriteFromState(SpriteState source, UISelectionState state)
        {
            return state switch
            {
                UISelectionState.Normal => null,
                UISelectionState.Highlighted => spriteState.highlightedSprite,
                UISelectionState.Pressed => spriteState.pressedSprite,
                UISelectionState.Disabled => spriteState.disabledSprite,
                UISelectionState.Selected => spriteState.selectedSprite,
                _ => null,
            };
        }

        private void TextStateTransition(UISelectionState state, bool instant)
        {
            if (textLabel == null) return;

            Color fontColor = ColorFromState(fontColors, state);
            textLabel.target.CrossFadeColor(fontColor, instant ? 0f : fontColors.fadeDuration, true, true);
        }

        private void UpdateState()
        {
            selected = parent != null && parent.selectedItem == this;

            if (!interactable)
            {
                DoStateTransition(UISelectionState.Disabled, !Application.isPlaying || immediate);
            }
            else
            {
                DoStateTransition(pointerDown ? UISelectionState.Pressed : selected ? UISelectionState.Selected : hasPointer ? UISelectionState.Highlighted : UISelectionState.Normal, !Application.isPlaying || immediate);
            }

            immediate = false;
        }

        private IEnumerator WaitAndSubmit()
        {
            yield return new WaitForEndOfFrame();
            onSubmit?.Invoke(this);
        }

        #endregion

    }
}