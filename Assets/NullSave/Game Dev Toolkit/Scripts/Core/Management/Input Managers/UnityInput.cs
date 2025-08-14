using UnityEngine;

namespace NullSave.GDTK
{
    [CreateAssetMenu(menuName = "Tools/GDTK/Settings/Unity Input Manager", fileName = "Unity Input Manager")]
    public class UnityInput : InputManager
    {

        #region Public Methods

        public override float GetAxis(string axisName) { if (string.IsNullOrEmpty(axisName)) return 0; return Input.GetAxis(axisName); }

        public override bool GetButton(string buttonName) { return Input.GetButton(buttonName); }

        public override bool GetButtonDown(string buttonName) { return Input.GetButtonDown(buttonName); }

        public override bool GetButtonUp(string buttonName) { return Input.GetButtonUp(buttonName); }

        public override bool GetKey(KeyCode key) { return Input.GetKey(key); }

        public override bool GetKeyDown(KeyCode key) { return Input.GetKeyDown(key); }

        public override bool GetKeyUp(KeyCode key) { return Input.GetKeyUp(key); }

        #endregion

    }
}