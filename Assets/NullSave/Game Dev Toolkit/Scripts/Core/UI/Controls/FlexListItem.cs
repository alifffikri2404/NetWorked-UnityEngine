using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    public class FlexListItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {

        #region Fields

        public Image image;
        public Label label;

        public Graphic background;
        [Tooltip("Colors used on text component")] [SerializeField] private ColorBlock m_colors;

        public UnityEvent onSelected;
        public UnityEvent onDeselected;
        public UnityEvent onClick;

        private bool m_selected;

        #endregion

        #region Properties

        public ColorBlock colors
        {
            get { return m_colors; }
            set
            {
                m_colors = value;
                UpdateState();
            }
        }

        public bool hasPointer { get; private set; }

        public bool pointerDown { get; private set; }

        public virtual bool selected
        {
            get { return m_selected; }
            set
            {
                if (m_selected == value) return;
                m_selected = value;
                if (m_selected)
                {
                    onSelected?.Invoke();
                }
                else
                {
                    onDeselected?.Invoke();
                }
                UpdateState();
            }
        }

        #endregion

        #region Unity Methods

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke();
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

        private void Reset()
        {
            background = GetComponent<Graphic>();
            m_colors = new ColorBlock();
            m_colors.colorMultiplier = 1;
            m_colors.normalColor = Color.white;
            m_colors.highlightedColor = new Color(0.9607843f, 0.9607843f, 0.9607843f);
            m_colors.pressedColor = new Color(0.7843137f, 0.7843137f, 0.7843137f);
            m_colors.selectedColor = new Color(0, 0.4734311f, 1);
            m_colors.disabledColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5019608f);
            m_colors.fadeDuration = 0.1f;
        }

        #endregion

        #region Private Methods

        protected virtual void DoStateTransition(UISelectionState state, bool instant)
        {
            if (colors == null || background == null) return;

            var tintColor = state switch
            {
                UISelectionState.Normal => colors.normalColor,
                UISelectionState.Highlighted => colors.highlightedColor,
                UISelectionState.Pressed => colors.pressedColor,
                UISelectionState.Disabled => colors.disabledColor,
                UISelectionState.Selected => colors.selectedColor,
                _ => Color.black,
            };

            background.CrossFadeColor(tintColor, instant ? 0f : colors.fadeDuration, true, true);
        }

        private void UpdateState()
        {
            DoStateTransition(pointerDown ? UISelectionState.Pressed : selected ? UISelectionState.Selected : hasPointer ? UISelectionState.Highlighted : UISelectionState.Normal, !Application.isPlaying);
        }

        #endregion

    }
}
