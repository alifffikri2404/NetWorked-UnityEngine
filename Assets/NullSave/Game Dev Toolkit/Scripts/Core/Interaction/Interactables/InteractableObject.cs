using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    public class InteractableObject : MonoBehaviour
    {

        #region Fields

        [Tooltip("Determines if this object is interactable")] public bool interactable;
        [Tooltip("Text associated with this action")] public string actionText;
        [Tooltip("Image associated with the action")] public Sprite actionImage;

        [Tooltip("Display message when object is not interactable")] public bool showAltText;
        [Tooltip("Text associated with this action when not interactable")] public string alternateText;

        [Tooltip("Custom UI to use instead of the normal Interactor UI")] public InteractorUI customUI;

        [Tooltip("Name of the broadcaster channel to use with the audio ppol")] public string audioPoolChannel;
        [Tooltip("Sound to play when interacting")] public AudioClip actionSound;

        public UnityEvent onInteract;

        #endregion

        #region Properties

        public virtual Interactor CurrentAgent { get; set; }

        public virtual Sprite InteractionImage
        {
            get { return actionImage; }
        }

        public virtual string InteractionText
        {
            get
            {
                if (!IsInteractable && showAltText)
                {
                    return alternateText;
                }
                return actionText;
            }
        }

        public virtual bool IsInteractable
        {
            get
            {
                return interactable;
            }
            set
            {
                interactable = value;
            }
        }

        #endregion

        #region Unity Methods

        public virtual void Reset()
        {
            interactable = true;
        }

        #endregion

        #region Public Methods

        public virtual bool Interact(Interactor source)
        {
            if (!IsInteractable) return false;

            Broadcaster.Broadcast(audioPoolChannel, "Play", new object[] { actionSound, transform.position });
            onInteract?.Invoke();
            return true;
        }

        public void SetInteractable(bool canInteract)
        {
            IsInteractable = canInteract;
        }

        #endregion

    }
}