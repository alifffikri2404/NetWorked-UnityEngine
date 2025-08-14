using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    public class InteractorUI : MonoBehaviour
    {

        #region Fields

        [Tooltip("Destination of the text")] public Label uiText;
        [Tooltip("Format to use when setting text. {0} is replaced with the supplied interactable object text")] public string format;
        [Tooltip("Require button/key to be held to interact")] public bool requireHold;
        [Tooltip("Seconds to require hold before interaction")] public float holdTime;
        [Tooltip("Progressbar for hold time")] public Progressbar holdProgressbar;

        public UnityEvent onHeldTimeChanged;

        private float m_timeHeld;

        #endregion

        #region Properties

        public bool holding { get; set; }

        public Interactor source { get; set; }

        public InteractableObject target { get; set; }

        public float timeHeld
        {
            get { return m_timeHeld; }
            set
            {
                m_timeHeld = value;
                onHeldTimeChanged?.Invoke();
                if(holdProgressbar != null)
                {
                    holdProgressbar.value = m_timeHeld / holdTime;
                }
            }
        }

        #endregion

        #region Unity Methods

        public virtual void OnEnable()
        {
            timeHeld = 0;
        }

        public virtual void Reset()
        {
            format = "{0}";
            holdTime = 1;
        }

        #endregion

        #region Public Methods

        public virtual void Initialize() { }

        public virtual void InteractWithTarget()
        {
            if (target != null && source != null) target.Interact(source);
        }

        public virtual void SetText(string value)
        {
            if (uiText == null) return;
            uiText.text = format.Replace("{0}", value);
        }

        #endregion

    }
}