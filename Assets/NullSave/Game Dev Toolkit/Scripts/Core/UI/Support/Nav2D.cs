using System;
using UnityEngine;

namespace NullSave.GDTK
{
    [Serializable]
    public class Nav2D
    {

        #region Fields

        [Tooltip("Allowed input methods")] public InputFlags allowedInput;
        [Tooltip("Horizontal Axis")] public string navHorizontal;
        [Tooltip("Vertical Axis")] public string navVertical;
        [Tooltip("Left key")] public KeyCode leftKey;
        [Tooltip("Right key")] public KeyCode rightKey;
        [Tooltip("Up key")] public KeyCode upKey;
        [Tooltip("Down key")] public KeyCode downKey;
        [Tooltip("Automatically repeat input")] public bool autoRepeat;
        [Tooltip("Seconds to wait between repeats")] public float repeatDelay;
        [Tooltip("Ignore all input")] public bool lockInput;
        [Tooltip("Allow moving from last to first or vise versa")] public bool allowAutoWrap;

        public SimpleEvent onNextH, onBackH;
        public SimpleEvent onNextV, onBackV;

        private KeyCode repeatKey;
        private string repeatAxis;
        private float nextRepeat;
        private bool waitForZero;

        #endregion

        #region Constructor

        public Nav2D()
        {
            navHorizontal = "Horizontal";
            navVertical = "Vertical";
            leftKey = KeyCode.LeftArrow;
            rightKey = KeyCode.RightArrow;
            upKey = KeyCode.UpArrow;
            downKey = KeyCode.DownArrow;
            autoRepeat = true;
            repeatDelay = 0.25f;
            allowedInput = (InputFlags)7;
        }

        #endregion

        #region Public Methods

        public void Update(float deltaTime)
        {
            if (lockInput) return;

            // Key Navigation
            if (allowedInput.HasFlag(InputFlags.Key))
            {
                if (KeyNav(leftKey, deltaTime, onBackH)) return;
                if (KeyNav(rightKey, deltaTime, onNextH)) return;
                if (KeyNav(upKey, deltaTime, onBackV)) return;
                if (KeyNav(downKey, deltaTime, onNextV)) return;
            }

            // Axis Navigation
            if (allowedInput.HasFlag(InputFlags.Axis))
            {
                if (AxisNav(navHorizontal, deltaTime, onBackH, onNextH)) return;
                if (AxisNav(navVertical, deltaTime, onNextV, onBackV)) return;
            }

            waitForZero = false;
            nextRepeat = 0;
        }

        #endregion

        #region Private Methods

        private bool AxisNav(string axisName, float deltaTime, SimpleEvent backEvent, SimpleEvent nextEvent)
        {
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