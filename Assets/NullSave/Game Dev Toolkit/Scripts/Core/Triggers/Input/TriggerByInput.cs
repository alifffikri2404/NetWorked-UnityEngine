using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    public class TriggerByInput : MonoBehaviour
    {

        #region Fields

        public string buttonName;
        public KeyCode keyCode;
        private bool badButton;

        public UnityEvent onTriggered;

        #endregion

        #region Unity Methods

        private void Update()
        {
            if (InterfaceManager.Input.GetKeyDown(keyCode)) onTriggered?.Invoke();

            if (!badButton && !string.IsNullOrEmpty(buttonName))
            {
                try
                {
                    if (InterfaceManager.Input.GetButtonDown(buttonName))
                    {
                        onTriggered?.Invoke();
                    }
                }
                catch
                {
                    StringExtensions.LogError("TriggerByInput", "Invoked", "Invalid button name: " + buttonName + "; disabling checks");
                    badButton = true;
                }
            }
        }

        #endregion

    }
}