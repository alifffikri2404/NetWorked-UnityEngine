using System;
using UnityEngine;

namespace NullSave.GDTK
{
    [Serializable]
    public class UI1DNav
    {

        #region Fields

        [Tooltip("Allowed input methods")] public InputFlags allowedInput;
        [Tooltip("Axis used for navigation")] public string navAxis;
        [Tooltip("Back key")] public KeyCode backKey;
        [Tooltip("Next key")] public KeyCode nextKey;
        [Tooltip("Automatically repeat input")] public bool autoRepeat;
        [Tooltip("Seconds to wait between repeats")] public float repeatDelay;
        [Tooltip("Ignore all input")] public bool lockInput;
        [Tooltip("Allow moving from last to first or vise versa")] public bool allowAutoWrap;

        [Tooltip("Event raised on next")] public SimpleEvent onNext;
        [Tooltip("Event raised on back")] public SimpleEvent onBack;

        private KeyCode repeatKey;
        private string repeatAxis;
        private float nextRepeat;
        private bool waitForZero;

        #endregion

        #region Constructor

        public UI1DNav()
        {
            navAxis = "Horizontal";
            backKey = KeyCode.Z;
            nextKey = KeyCode.C;
            autoRepeat = true;
            repeatDelay = 0.25f;
            allowedInput = (InputFlags)7;
        }

        #endregion

        #region Public Methods

        public void Update(float deltaTime)
        {
            if (lockInput) return;

            // Key Nav
            if (allowedInput.HasFlag(InputFlags.Key))
            {
                if (KeyNav(backKey, deltaTime, onBack)) return;
                if (KeyNav(nextKey, deltaTime, onNext)) return;
            }

            // Nav Axis
            if (allowedInput.HasFlag(InputFlags.Axis))
            {
                if (AxisNav(navAxis, deltaTime, onBack, onNext)) return;
            }

            waitForZero = false;
            nextRepeat = 0;
        }

        #endregion

        #region Private Methods

        private bool AxisNav(string axisName, float deltaTime, SimpleEvent backEvent, SimpleEvent nextEvent)
        {
            if (!Application.isPlaying) return false;

            float axis = InterfaceManager.Input.GetAxis(axisName);

            if (axis <= -0.1f)
            {
                nextRepeat -= deltaTime;

                if (nextRepeat <= 0)
                {
                    backEvent?.Invoke();
                    waitForZero = !autoRepeat;
                    repeatAxis = axisName;
                    nextRepeat += repeatDelay;
                }

                return true;
            }

            if (axis >= 0.1f)
            {
                nextRepeat -= deltaTime;

                if (nextRepeat <= 0)
                {
                    nextEvent?.Invoke();
                    waitForZero = !autoRepeat;
                    repeatAxis = axisName;
                    nextRepeat += repeatDelay;
                }

                return true;
            }

            return false;
        }

        private bool KeyNav(KeyCode key, float deltaTime, SimpleEvent navEvent)
        {
            if (!Application.isPlaying) return false;

            if (InterfaceManager.Input.GetKeyDown(key))
            {
                navEvent?.Invoke();
                repeatKey = key;
                nextRepeat = repeatDelay;
                return true;
            }

            if (autoRepeat && repeatKey == key)
            {
                if (InterfaceManager.Input.GetKey(key))
                {
                    nextRepeat -= deltaTime;
                    if (nextRepeat <= 0)
                    {
                        nextRepeat += repeatDelay;
                        navEvent?.Invoke();
                    }
                    return true;
                }
                else
                {
                    repeatKey = KeyCode.None;
                }
            }

            return false;
        }

        #endregion

    }
}
