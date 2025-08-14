using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    public class InteractableSwitch : InteractableObject
    {

        #region Fields

        [Tooltip("Boolean parameter to set on the animator")] public string onBoolAnim;
        [Tooltip("Determines if the switch is on")] [SerializeField] private bool isOn;

        [Tooltip("Text to display when the switch can be activated")] public string activateText;
        [Tooltip("Text to displayw hen the switch can be deactivated")] public string deactivateText;

        public UnityEvent onSwitchOn, onSwitchOff;

        #endregion

        #region Properties

        public Animator Animator { get; private set; }

        public override string InteractionText
        {
            get
            {
                if (isOn)
                {
                    return deactivateText;
                }
                return activateText;
            }
        }

        public bool IsOn
        {
            get { return isOn; }
            set
            {
                if (IsOn == value) return;

                isOn = value;
                if (Animator != null)
                {
                    Animator.SetBool(onBoolAnim, value);
                }

                RaiseEvents();
            }
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        public override void Reset()
        {
            base.Reset();
            onBoolAnim = "On";
        }

        private void Start()
        {
            RaiseEvents(false);
        }

        #endregion

        #region Public Methods

        public override bool Interact(Interactor soure)
        {
            if (!IsInteractable) return false;
            IsOn = !isOn;
            onInteract?.Invoke();
            return true;
        }

        #endregion

        #region Private Methods

        private void RaiseEvents(bool includeSounds = true)
        {
            if (IsOn)
            {
                onSwitchOn?.Invoke();
            }
            else
            {
                onSwitchOff?.Invoke();
            }

            if (includeSounds)
            {
                Broadcaster.Broadcast(audioPoolChannel, "Play", new object[] { actionSound, transform.position });
            }
        }

        #endregion

    }
}