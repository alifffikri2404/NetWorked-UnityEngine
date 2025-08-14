#if ENABLE_INPUT_SYSTEM
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace NullSave.GDTK
{
    public class ActionIcons : MonoBehaviour
    {

        #region Constants

        private const float refreshRate = 0.05f;

        #endregion

        #region Fields

        [Tooltip("List of available controller maps")] public List<ControllerMap> controllerMaps;
        [Tooltip("Event raised whenever the active controller changes")] public UnityEvent onControllerChanged;

        private float elapsed;
        private static ActionIcons m_instance;

        #endregion

        #region Properties

        /// <summary>
        /// Returns the current instance
        /// </summary>
        public static ActionIcons instance
        {
            get
            {
                if(m_instance == null)
                {
                    // Check if something called us before we awoke.
                    m_instance = FindObjectOfType<ActionIcons>();
                    if (m_instance != null) return m_instance;

                    GameObject go = new GameObject("GDTK Action Icons");
                    m_instance = go.AddComponent<ActionIcons>();
                    m_instance.Reset();
                }

                return m_instance;
            }
        }

        /// <summary>
        /// Returns the last active device
        /// </summary>
        public InputDevice lastActiveDevice { get; private set; }

        #endregion

        #region Unity Methods

        // Instance and create lists
        private void Awake()
        {
            if (m_instance != null && m_instance != this)
            {
                Destroy(gameObject);
                return;
            }

            if (m_instance == null)
            {
                m_instance = this;
                this.transform.SetParent(null);
                DontDestroyOnLoad(this);
            }
        }

        private void Update()
        {
            elapsed += Time.deltaTime;
            if (elapsed >= refreshRate)
            {
                elapsed -= refreshRate;
                UpdateActiveController();
            }
        }

        private void Reset()
        {
            onControllerChanged = new UnityEvent();

            controllerMaps = new List<ControllerMap>();
            controllerMaps.Add(Resources.Load<ControllerMap>("Controller Maps/Keyboard and Mouse"));
            controllerMaps.Add(Resources.Load<ControllerMap>("Controller Maps/XBOX Controller"));
            controllerMaps.Add(Resources.Load<ControllerMap>("Controller Maps/PlayStation Controller"));
            controllerMaps.Add(Resources.Load<ControllerMap>("Controller Maps/Switch Controller"));
            controllerMaps.Add(Resources.Load<ControllerMap>("Controller Maps/Generic Controller"));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the icon associated with an action
        /// </summary>
        /// <param name="inputActions"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public static InputMap GetActionIcon(InputActionAsset inputActions, string actionName)
        {
            if (inputActions == null) return null;

            string modifier = GetModifier(ref actionName).Trim().ToLower();

            InputMap result = GetActionIcon(inputActions, actionName, modifier, false);
            if (result != null) return result;

            return GetActionIcon(inputActions, actionName, modifier, true);
        }

        #endregion

        #region Private Methods

        private static InputMap GetActionIcon(InputActionAsset inputActions, string actionName, string modifier, bool fallback)
        {
            InputMap result = null;
            string compositeName = string.Empty;
            bool inComposite = false;

            foreach (InputActionMap map in inputActions.actionMaps)
            {
                if (!map.enabled) map.Enable();

                foreach (InputAction action in map.actions)
                {
                    if (action.name == actionName)
                    {
                        foreach (InputBinding binding in action.bindings)
                        {
                            if (binding.isPartOfComposite)
                            {
                                if (!inComposite)
                                {
                                    inComposite = true;
                                    compositeName = "/" + binding.path.Substring(binding.path.IndexOf('/') + 1);
                                }
                                else
                                {
                                    compositeName += "+" + binding.path.Substring(binding.path.IndexOf('/') + 1);
                                }
                            }
                            else
                            {
                                if (inComposite)
                                {
                                    inComposite = false;
                                    result = instance.GetMapForBinding(compositeName, modifier, fallback);
                                    if (result != null) return result;

                                    result = instance.GetMapForBinding(binding.path, modifier, fallback);
                                    if (result != null) return result;

                                }
                                else
                                {
                                    result = instance.GetMapForBinding(binding.path, modifier, fallback);
                                    if (result != null) return result;
                                }
                            }

                        }
                    }
                }
            }

            if (inComposite)
            {
                result = instance.GetMapForBinding(compositeName, modifier, fallback);
            }

            return result;
        }

        private InputMap GetMapForBinding(string bindingName, string modifier, bool fallback = false)
        {
            InputMap result = null;
            string actionPath = bindingName.Substring(bindingName.IndexOf('/') + 1);

            GetModifiedActionName(ref actionPath, modifier);

            foreach (ControllerMap map in controllerMaps)
            {
                if (fallback)
                {
                    if (map.HasAction(actionPath) && map.isFallback)
                    {
                        return map.GetAction(actionPath);
                    }
                }
                else if (lastActiveDevice != null)
                {
                    if (map.compatibleDevices.Contains(lastActiveDevice.name) && map.HasAction(actionPath))
                    {
                        return map.GetAction(actionPath);
                    }
                }
            }

            return result;
        }

        private static string GetModifier(ref string actionName)
        {
            int i = actionName.IndexOf(":");
            if (i <= 0) return string.Empty;

            string result = actionName.Substring(i + 1);
            actionName = actionName.Substring(0, i);

            return result;
        }

        private void GetModifiedActionName(ref string actionName, string modifier)
        {
            int i;

            switch (modifier)
            {
                case "neg":
                    i = actionName.IndexOf("+");
                    if (i >= 0)
                    {
                        actionName = actionName.Substring(0, i);
                    }
                    break;
                case "pos":
                    i = actionName.IndexOf("+");
                    if (i >= 0)
                    {
                        actionName = actionName.Substring(i + 1);
                    }
                    break;
            }
        }

        private void UpdateActiveController()
        {
            double bestUpdateTime = 0;
            InputDevice bestDevice = lastActiveDevice;

            foreach (var device in InputSystem.devices)
            {
                if (device.enabled)
                {
                    if (device.lastUpdateTime > bestUpdateTime)
                    {
                        bestUpdateTime = device.lastUpdateTime;
                        bestDevice = device;
                    }
                }
            }

            if (lastActiveDevice != bestDevice)
            {
                lastActiveDevice = bestDevice;
                onControllerChanged?.Invoke();
            }
        }

        #endregion

    }
}
#endif