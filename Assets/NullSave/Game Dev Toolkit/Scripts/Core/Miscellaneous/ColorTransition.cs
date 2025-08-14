using UnityEngine;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    [RequireComponent(typeof(Graphic))]
    public class ColorTransition : MonoBehaviour
    {

        #region Fields

        [Tooltip("Colors used on text component")] [SerializeField] private ColorBlock m_colors;

        private Graphic target;
        private bool m_selected;
        private bool m_pointedDown;
        private bool m_hasPointer;

        #endregion

        #region Properties

        public ColorBlock colors
        {
            get { return m_colors; }
            set
            {
                if (m_colors.Equals(value)) return;
                m_colors = value;
                UpdateState();
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    UnityEditor.EditorUtility.SetDirty(this);
                }
#endif
            }
        }

        public bool hasPointer
        {
            get { return m_hasPointer; }
            set
            {
                if (m_hasPointer == value) return;
                m_hasPointer = value;
                UpdateState();
            }
        }

        public bool pointerDown
        {
            get { return m_pointedDown; }
            set
            {
                if (m_pointedDown == value) return;
                m_pointedDown = value;
                UpdateState();
            }
        }

        public bool selected
        {
            get { return m_selected; }
            set
            {
                if (m_selected == value) return;
                m_selected = value;
                UpdateState();
            }
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            target = GetComponent<Graphic>();
            UpdateState();
        }

        private void OnEnable()
        {
            UpdateState();
        }

        #endregion

        #region Public Methods

        public void ChangeColors(ColorTransition target)
        {
            ColorBlock cb = new ColorBlock();
            cb.colorMultiplier = 1;
            cb.disabledColor = Color.black;
            cb.fadeDuration = 0.1f;
            cb.highlightedColor = Color.blue;
            cb.normalColor = Color.white;
            cb.pressedColor = Color.green;
            cb.selectedColor = Color.red;
            target.colors = cb;
        }

        #endregion

        #region Private Methods

        private void DoStateTransition(UISelectionState state, bool instant)
        {
            if (colors == null) return;
            var tintColor = state switch
            {
                UISelectionState.Normal => colors.normalColor,
                UISelectionState.Highlighted => colors.highlightedColor,
                UISelectionState.Pressed => colors.pressedColor,
                UISelectionState.Disabled => colors.disabledColor,
                UISelectionState.Selected => colors.selectedColor,
                _ => Color.black,
            };
            if (gameObject.activeInHierarchy)
            {
                target.CrossFadeColor(tintColor, instant ? 0f : colors.fadeDuration, true, true);
            }
        }

        private void UpdateState()
        {
            //if (!interactable)
            //{
            //    DoStateTransition(UISelectionState.Disabled, !Application.isPlaying);
            //}
            //else
            //{
                DoStateTransition(pointerDown ? UISelectionState.Pressed : selected ? UISelectionState.Selected : hasPointer ? UISelectionState.Highlighted : UISelectionState.Normal, !Application.isPlaying);
            //}
        }


        #endregion

    }
}