using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    [AutoDoc("This component raises an event whenever the specified button conditions are met.")]
    public class TriggerByButton : MonoBehaviour
    {

        #region Fields

        [Tooltip("Name of the button to listen to")] public string button;
        [Tooltip("Raise event on button")] public bool onButton;
        [Tooltip("Raise event on button down")] public bool onButtonDown;
        [Tooltip("Raise event on button up")] public bool onButtonUp;

        [Tooltip("Event raised when conditions are met")] public UnityEvent onTrigger;

        #endregion

        #region Unity Methods

        private void Update()
        {
            if (onButton && InterfaceManager.Input.GetButton(button)) onTrigger?.Invoke();
            if (onButtonDown && InterfaceManager.Input.GetButtonDown(button)) onTrigger?.Invoke();
            if (onButtonUp && InterfaceManager.Input.GetButtonUp(button)) onTrigger?.Invoke();
        }

        #endregion

    }
}