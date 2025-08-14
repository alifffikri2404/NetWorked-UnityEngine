using UnityEngine;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    [AutoDoc("UI control used to display progress.")]
    [ExecuteInEditMode]
    public class Progressbar : MonoBehaviour
    {

        #region Enumerations

        public enum Direction
        {
            LeftToRight = 0,
            RightToLeft = 1,
            BottomToTop = 2,
            TopToBottom = 3
        }

        public enum FillMode
        {
            ImageSize = 0,
            ImageFill = 1
        }

        #endregion

        #region Fields

        [SerializeField][Tooltip("Graphic used to display fill")] private Image _targetGraphic;
        [SerializeField] [Tooltip("Direction of fill")] private Direction _direction;
        [SerializeField] [Tooltip("Mode used to fill progressbar")] private FillMode _fillMode;

        [SerializeField] [Tooltip("Minimum allowed value")] private float _minValue;
        [SerializeField] [Tooltip("Maximum allowed value")] private float _maxValue;
        [SerializeField] [Tooltip("Current value")] private float _value;

        [Tooltip("Event raised when value changes")] public ValueChanged onValueChanged;

        private RectTransform m_rtFill;

        #endregion

        #region Properties

        [AutoDoc("Direction of fill.", "sampleObject.direction = Progresbar.Direction.LeftToRight;")]
        public Direction direction
        {
            get { return _direction; }
            set
            {
                if (_direction == value) return;

                // Swap height/width
                bool wasHorz = _direction == Direction.LeftToRight || _direction == Direction.RightToLeft;
                bool isHorz = value == Direction.LeftToRight || value == Direction.RightToLeft;
                if(wasHorz != isHorz)
                {
                    RectTransform rt = GetComponent<RectTransform>();
                    rt.sizeDelta = new Vector2(rt.sizeDelta.y, rt.sizeDelta.x);
                }

                _direction = value;
                UpdateDirection();
            }
        }

        [AutoDoc("Mode used to fill progressbar.", "sampleObject.fillMode = Progresbar.FillMode.ImageFill;")]
        public FillMode fillMode
        {
            get { return _fillMode; }
            set
            {
                if (_fillMode == value) return;
                _fillMode = value;
                UpdateFillMode();
            }
        }

        [AutoDoc("Maximum allowed value.", "sampleObject.maxValue = 20;")]
        public float maxValue
        {
            get { return _maxValue; }
            set
            {
                if (_maxValue == value) return;
                _maxValue = value;
                UpdateProgress();
            }
        }

        [AutoDoc("Minimum allowed value", "sampleObject.minValue = 0;")]
        public float minValue
        {
            get { return _minValue; }
            set
            {
                if (_minValue == value) return;
                _minValue = value;
                UpdateProgress();
            }
        }

        private RectTransform rtFill
        {
            get
            {
                if(m_rtFill == null)
                {
                    m_rtFill = targetGraphic.GetComponent<RectTransform>();
                }
                return m_rtFill;
            }
        }

        [AutoDoc("Graphic used to display fill", "sampleObject.targetGraphic.fillClockwise = false;")]
        public Image targetGraphic
        {
            get { return _targetGraphic; }
            set
            {
                if (_targetGraphic == value) return;
                _targetGraphic = value;
                UpdateFillMode();
            }
        }

        [AutoDoc("Current value", "sampleObject.value = 11;")]
        public float value
        {
            get { return _value; }
            set
            {
                value = Mathf.Clamp(value, minValue, maxValue);
                if (_value == value) return;
                _value = value;
                UpdateProgress();
            }
        }

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if(targetGraphic != null)
            {
                UpdateFillMode();
            }
        }

        private void Reset()
        {
            maxValue = 1;
        }

        #endregion

        #region Private Methods

        private void UpdateDirection()
        {
            switch(_direction)
            {
                case Direction.BottomToTop:
                    rtFill.anchorMax = new Vector2(1, 0);
                    rtFill.anchorMin = Vector2.zero;
                    rtFill.pivot = new Vector2(1, 0);
                    break;
                case Direction.LeftToRight:
                    rtFill.anchorMax = new Vector2(0, 1);
                    rtFill.anchorMin = Vector2.zero;
                    rtFill.pivot = new Vector2(0, 1);
                    break;
                case Direction.RightToLeft:
                    rtFill.anchorMax = new Vector2(1, 1);
                    rtFill.anchorMin = Vector2.zero;
                    rtFill.pivot = new Vector2(1, 1);
                    break;
                case Direction.TopToBottom:
                    rtFill.anchorMax = new Vector2(1, 1);
                    rtFill.anchorMin = Vector2.zero;
                    rtFill.pivot = new Vector2(1, 1);
                    break;

            }

            UpdateProgress();
        }

        private void UpdateFillMode()
        {
            if (targetGraphic == null) return;

            switch(_fillMode)
            {
                case FillMode.ImageFill:
                    rtFill.anchorMax = new Vector2(1, 1);
                    rtFill.anchorMin = rtFill.offsetMin = rtFill.offsetMax = Vector2.zero;
                    rtFill.pivot = new Vector2(0.5f, 0.5f);
                    targetGraphic.type = Image.Type.Filled;
                    UpdateProgress();
                    break;
                case FillMode.ImageSize:
                    targetGraphic.type = Image.Type.Sliced;
                    UpdateDirection();
                    break;
            }
        }

        private void UpdateProgress()
        {
            if (targetGraphic == null) return;


            float percent = _maxValue == _minValue ? 1 : Mathf.Clamp((_value - _minValue) / (_maxValue - _minValue), 0, 1);

            switch (_fillMode)
            {
                case FillMode.ImageFill:
                    targetGraphic.fillAmount = percent;
                    break;
                case FillMode.ImageSize:
                    switch(_direction)
                    {
                        case Direction.BottomToTop:
                            rtFill.anchorMax = new Vector2(1, percent);
                            break;
                        case Direction.LeftToRight:
                            rtFill.anchorMax = new Vector2(percent, 1);
                            break;
                        case Direction.RightToLeft:
                            rtFill.anchorMin = new Vector2(1 - percent, 0);
                            break;
                        case Direction.TopToBottom:
                            rtFill.anchorMin = new Vector2(0, 1 - percent);
                            break;
                    }
                    rtFill.offsetMax = rtFill.offsetMin = Vector2.zero;

                    break;
            }

#if UNITY_EDITOR
            if (!Application.isPlaying && targetGraphic != null)
            {
                UnityEditor.EditorUtility.SetDirty(targetGraphic);
            }
#endif
        }

        #endregion

    }
}