using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    [AutoDoc("A UI control used to display a list of options in a single line, showing only the currently selected value and navigating with arrows.")]
    public class InlineList : Selectable
    {

        #region Fields

        [Tooltip("Image showing the left navigation arrow.")] public Image leftArrow;
        [Tooltip("Label displaying the selected item text.")] public Label optionText;
        [Tooltip("Image showing the right navigation arrow.")] public Image rightArrow;

        [Tooltip("Allow moving from the last to first item or vice versa.")] public bool allowLooping;
        [Tooltip("List of available options")] public List<string> options;

        [Tooltip("Method used to navigation between options.")] public NavigationTypeSimple inputType;
        [Tooltip("Button used to navigation between options.")] public string inputButton;
        [Tooltip("Key used to go to previous item.")] public KeyCode previousKey;
        [Tooltip("Key used to go to next time.")] public KeyCode nextKey;

        [Tooltip("Event raised whenever selection changes.")] public UnityEvent onSelectionChanged;

        private int selIndex;

        private SelectionState m_CurrentSelectionState;
        private bool hasSelection;
        private bool waitForUp;

        #endregion

        #region Properties

        [AutoDoc("Check if pointer is down on control.", "Debug.Log(sampleObject.pointerDown);")]
        private bool pointerDown { get; set; }

        [AutoDoc("Check if pointer is over control.", "Debug.Log(sampleObject.pointerInside);")]
        private bool pointerInside { get; set; }

        [AutoDoc("Gets/Sets the selected index.", "sampleObject.selectedIndex = 0;")]
        public int selectedIndex
        {
            get { return selIndex; }
            set
            {
                if (value < 0) value = allowLooping ? options.Count - 1 : 0;
                if (value >= options.Count) value = allowLooping ? 0 : options.Count - 1;
                if (value == selIndex) return;

                selIndex = value;
                if (optionText)
                {
                    if (selIndex > -1 && selIndex < options.Count)
                    {
                        optionText.text = options[selIndex];
                    }
                    else
                    {
                        optionText.text = string.Empty;
                    }
                }
                InternalEvaluateAndTransitionToSelectionState(false);

                onSelectionChanged?.Invoke();
            }
        }

        [AutoDoc("Gets the text for the selected index.", "Debug.Log(sampleObject.selectedText);")]
        public string selectedText
        {
            get
            {
                if (selIndex == -1) return null;
                return options[selIndex];
            }
        }

        #endregion

        #region Unity Methods

        [AutoDocSuppress]
        protected override void Start()
        {
            if (options == null) options = new List<string>();
            selIndex = -1;
            selectedIndex = 0;
        }

        [AutoDocSuppress]
        protected override void OnEnable()
        {
            base.OnEnable();

            var state = SelectionState.Normal;

            // The button will be highlighted even in some cases where it shouldn't.
            // For example: We only want to set the State as Highlighted if the StandaloneInputModule.m_CurrentInputMode == InputMode.Buttons
            // But we dont have access to this, and it might not apply to other InputModules.
            // TODO: figure out how to solve this. Case 617348.
            if (hasSelection)
                state = SelectionState.Highlighted;

            m_CurrentSelectionState = state;
            InternalEvaluateAndTransitionToSelectionState(true);
        }

        [AutoDocSuppress]
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            // Selection tracking
            if (IsInteractable() && navigation.mode != Navigation.Mode.None)
                EventSystem.current.SetSelectedGameObject(gameObject, eventData);

            if (eventData.pointerCurrentRaycast.gameObject == leftArrow.gameObject)
            {
                selectedIndex -= 1;
            }
            else if (eventData.pointerCurrentRaycast.gameObject == rightArrow.gameObject)
            {
                selectedIndex += 1;
            }

            pointerDown = true;
            EvaluateAndTransitionToSelectionState(eventData);
        }

        [AutoDocSuppress]
        public override void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            pointerDown = false;
            EvaluateAndTransitionToSelectionState(eventData);
        }

        [AutoDocSuppress]
        public override void OnPointerEnter(PointerEventData eventData)
        {
            pointerInside = true;
            EvaluateAndTransitionToSelectionState(eventData);
        }

        [AutoDocSuppress]
        public override void OnPointerExit(PointerEventData eventData)
        {
            pointerInside = false;
            EvaluateAndTransitionToSelectionState(eventData);
        }

        [AutoDocSuppress]
        public override void OnSelect(BaseEventData eventData)
        {
            hasSelection = true;
            EvaluateAndTransitionToSelectionState(eventData);
        }

        [AutoDocSuppress]
        public override void OnDeselect(BaseEventData eventData)
        {
            hasSelection = false;
            EvaluateAndTransitionToSelectionState(eventData);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            //colors.fadeDuration = Mathf.Max(colors.fadeDuration, 0.0f);

            // OnValidate can be called before OnEnable, this makes it unsafe to access other components
            // since they might not have been initialized yet.
            // OnSetProperty potentially access Animator or Graphics. (case 618186)
            if (isActiveAndEnabled)
            {
                // Need to clear out the override image on the target...
                //DoSpriteSwap(null);

                // If the transition mode got changed, we need to clear all the transitions, since we don't know what the old transition mode was.
                StartColorTween(Color.white, true);
                //TriggerAnimation(m_AnimationTriggers.normalTrigger);

                // And now go to the right state.
                InternalEvaluateAndTransitionToSelectionState(true);
            }
        }
#endif

#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        protected void Reset()
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        {
            inputButton = "Horizontal";
            previousKey = KeyCode.LeftArrow;
            nextKey = KeyCode.RightArrow;
            inputType = NavigationTypeSimple.ByButton;
        }

        private void Update()
        {
            if (!hasSelection) return;

            switch(inputType)
            {
                case NavigationTypeSimple.ByButton:
                    float input = InterfaceManager.Input.GetAxis(inputButton);
                    if (waitForUp)
                    {
                        if (input != 0) return;
                        waitForUp = false;
                    }

                    if(input <= -0.1f)
                    {
                        selectedIndex -= 1;
                        waitForUp = true;
                    }
                    else if (input >= 0.1f)
                    {
                        selectedIndex += 1;
                        waitForUp = true;
                    }
                    break;
                case NavigationTypeSimple.ByKey:
                    if (InterfaceManager.Input.GetKeyDown(previousKey))
                    {
                        selectedIndex -= 1;
                    }
                    else if (InterfaceManager.Input.GetKeyDown(nextKey))
                    {
                        selectedIndex += 1;
                    }
                    break;
            }
        }

        #endregion

        #region Public Methods

        [AutoDoc("Adds a new option to the end of the list.", "sampleObject.AddOption(\"Hello World\");")]
        [AutoDocParameter("Value to add to list.")]
        public void AddOption(string value)
        {
            options.Add(value);
            if (selIndex == -1 || selIndex > options.Count - 1) selectedIndex = 0;
        }

        [AutoDoc("Adds a list of options to the end fo the list.", "sampleObject.AddOptions(optionList);")]
        [AutoDocParameter("List of values to add.")]
        public void AddOptions(List<string> values)
        {
            options.AddRange(values);
            if (selIndex == -1) selectedIndex = 0;
        }

        [AutoDoc("Remove all options from the list.", "sampleObject.Clear();")]
        public void Clear()
        {
            selectedIndex = -1;
            options.Clear();
            optionText.text = string.Empty;
        }

        [AutoDocSuppress]
        public override Selectable FindSelectableOnLeft()
        {
            return null;
        }

        [AutoDocSuppress]
        public override Selectable FindSelectableOnRight()
        {
            return null;
        }

        #endregion

        #region Private Methods

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (!gameObject.activeInHierarchy)
                return;
            var tintColor = state switch
            {
                SelectionState.Normal => colors.normalColor,
                SelectionState.Highlighted => colors.highlightedColor,
                SelectionState.Pressed => colors.pressedColor,
                SelectionState.Selected => colors.selectedColor,
                SelectionState.Disabled => colors.disabledColor,
                _ => Color.black,
            };
            StartColorTween(tintColor * colors.colorMultiplier, instant);
        }

        private void EvaluateAndTransitionToSelectionState(BaseEventData eventData)
        {
            if (!IsActive())
                return;

            UpdateSelectionState(eventData);
            InternalEvaluateAndTransitionToSelectionState(false);
        }

        private void InternalEvaluateAndTransitionToSelectionState(bool instant)
        {
            var transitionState = m_CurrentSelectionState;
            if (IsActive() && !IsInteractable())
                transitionState = SelectionState.Disabled;
            DoStateTransition(transitionState, instant);
        }

        protected bool IsHighlighted(BaseEventData eventData)
        {
            if (!IsActive())
                return false;

            if (IsPressed())
                return false;

            bool selected = hasSelection;
            if (eventData is PointerEventData)
            {
                var pointerData = eventData as PointerEventData;
                selected |=
                    (pointerDown && !pointerInside && pointerData.pointerPress == gameObject) // This object pressed, but pointer moved off
                    || (!pointerDown && pointerInside && pointerData.pointerPress == gameObject) // This object pressed, but pointer released over (PointerUp event)
                    || (!pointerDown && pointerInside && pointerData.pointerPress == null); // Nothing pressed, but pointer is over
            }
            else
            {
                selected |= pointerInside;
            }
            return selected;
        }

        void StartColorTween(Color targetColor, bool instant)
        {
            if (leftArrow != null)
            {
                if (allowLooping || selIndex > 0)
                {
                    leftArrow.CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
                }
                else
                {
                    leftArrow.CrossFadeColor(colors.disabledColor, instant ? 0f : colors.fadeDuration, true, true);
                }
            }
            if (rightArrow != null)
            {
                if (allowLooping || selIndex < options.Count - 1)
                {
                    rightArrow.CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
                }
                else
                {
                    rightArrow.CrossFadeColor(colors.disabledColor, instant ? 0f : colors.fadeDuration, true, true);
                }
            }
            if (optionText != null)
            {
                optionText.textMeshPro.CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
            }
        }

        protected void UpdateSelectionState(BaseEventData eventData)
        {
            if (IsPressed())
            {
                m_CurrentSelectionState = SelectionState.Pressed;
                return;
            }

            if (IsHighlighted(eventData))
            {
                m_CurrentSelectionState = SelectionState.Highlighted;
                return;
            }

            m_CurrentSelectionState = SelectionState.Normal;
        }

        #endregion

    }
}