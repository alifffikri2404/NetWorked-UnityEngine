using UnityEngine;

namespace NullSave.GDTK
{
    public class InputManager : ScriptableObject
    {

        #region Public Methods

        /// <summary>
        /// Returns true during the frame the user starts pressing down the key identified by name
        /// </summary>
        /// <param name="axisName"></param>
        /// <returns></returns>
        public virtual float GetAxis(string axisName) { throw new System.NotImplementedException(); }

        /// <summary>
        /// Returns true during the frame the user starts pressing down the key identified by name
        /// </summary>
        /// <param name="axisName"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public virtual float GetAxis(string axisName, int playerId) { throw new System.NotImplementedException(); }

        /// <summary>
        /// True when an axis has been pressed and not released
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public virtual bool GetButton(string buttonName) { throw new System.NotImplementedException(); }

        /// <summary>
        /// True when an axis has been pressed and not released
        /// </summary>
        /// <param name="buttonName"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public virtual bool GetButton(string buttonName, int playerId) { throw new System.NotImplementedException(); }

        /// <summary>
        /// Returns true during the frame the user pressed down the virtual button identified by buttonName
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public virtual bool GetButtonDown(string buttonName) { throw new System.NotImplementedException(); }

        /// <summary>
        /// Returns true during the frame the user pressed down the virtual button identified by buttonName
        /// </summary>
        /// <param name="buttonName"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public virtual bool GetButtonDown(string buttonName, int playerId) { throw new System.NotImplementedException(); }

        /// <summary>
        /// Returns true the first frame the user releases the virtual button identified by buttonName
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public virtual bool GetButtonUp(string buttonName) { throw new System.NotImplementedException(); }

        /// <summary>
        /// Returns true the first frame the user releases the virtual button identified by buttonName
        /// </summary>
        /// <param name="buttonName"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public virtual bool GetButtonUp(string buttonName, int playerId) { throw new System.NotImplementedException(); }

        /// <summary>
        /// Returns true while the user holds down the key identified by name
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool GetKey(KeyCode key) { throw new System.NotImplementedException(); }

        /// <summary>
        /// Returns true while the user holds down the key identified by name
        /// </summary>
        /// <param name="key"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public virtual bool GetKey(string key, int playerId) { throw new System.NotImplementedException(); }

        /// <summary>
        /// Returns true during the frame the user starts pressing down the key identified by name
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool GetKeyDown(KeyCode key) { throw new System.NotImplementedException(); }

        /// <summary>
        /// Returns true during the frame the user starts pressing down the key identified by name
        /// </summary>
        /// <param name="key"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public virtual bool GetKeyDown(string key, int playerId) { throw new System.NotImplementedException(); }

        /// <summary>
        /// Returns true during the frame the user releases the key identified by name
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool GetKeyUp(KeyCode key) { throw new System.NotImplementedException(); }

        /// <summary>
        /// Returns true during the frame the user releases the key identified by name
        /// </summary>
        /// <param name="key"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public virtual bool GetKeyUp(string key, int playerId) { throw new System.NotImplementedException(); }

        public virtual void Initialize() { }

        #endregion

    }
}