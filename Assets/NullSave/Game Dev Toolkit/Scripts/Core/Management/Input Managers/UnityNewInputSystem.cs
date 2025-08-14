#if ENABLE_INPUT_SYSTEM

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NullSave.GDTK
{
    [CreateAssetMenu(menuName = "Tools/GDTK/Settings/Unity New Input System", fileName = "Unity New Input System")]
    public class UnityNewInputSystem : InputManager
    {

        #region Fields

        public InputActionAsset inputActions;
        public string useMap;

        private Dictionary<string, InputAction> actions;

        #endregion

        #region Public Methods

        public override float GetAxis(string axisName)
        {
            if (!actions.ContainsKey(axisName)) return 0;

            return actions[axisName].ReadValue<float>();
        }

        public override bool GetButton(string buttonName)
        {
            if (!actions.ContainsKey(buttonName)) return false;

            return actions[buttonName].IsPressed();
        }

        public override bool GetButtonDown(string buttonName)
        {
            if (!actions.ContainsKey(buttonName)) return false;

            return actions[buttonName].WasPressedThisFrame();
        }

        public override bool GetButtonUp(string buttonName)
        {
            if (!actions.ContainsKey(buttonName)) return false;

            return actions[buttonName].WasReleasedThisFrame();
        }

        public override bool GetKey(KeyCode key) { return Input.GetKey(key); }

        public override bool GetKeyDown(KeyCode key) { return Input.GetKeyDown(key); }

        public override bool GetKeyUp(KeyCode key) { return Input.GetKeyUp(key); }

        public override void Initialize()
        {
            if (inputActions == null)
            {
                StringExtensions.LogError(name, "Initialize", "No Input Actions supplied");
                return;
            }

            inputActions.Enable();

            actions = new Dictionary<string, InputAction>();
            foreach (InputActionMap map in inputActions.actionMaps)
            {
                if (string.IsNullOrEmpty(useMap) || useMap == map.name)
                {
                    foreach (InputAction action in map.actions)
                    {
                        actions.Add(action.name, action);
                    }

                    break;
                }
            }
        }

        #endregion

    }
}

#endif