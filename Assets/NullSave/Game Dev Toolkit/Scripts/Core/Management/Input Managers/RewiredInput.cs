using System;
using System.Reflection;
using UnityEngine;

namespace NullSave.GDTK
{
    [CreateAssetMenu(menuName = "Tools/GDTK/Settings/ReWired Input Manager", fileName = "Rewired Input Manager")]
    public class RewiredInput : InputManager
    {

        #region Fields

        private Assembly asm;
        private object players;
        private MethodInfo getPlayer, getAxis, getButton, getButtonDown, getButtonUp;

        private object[] playerList = new object[20];

        #endregion

        #region Public Methods

        public override float GetAxis(string axisName) { return GetAxis(axisName, 0); }

        public override float GetAxis(string axisName, int playerId)
        {
            object player = GetPlayer(playerId);
            if (getAxis == null)
            {
                Type type = player.GetType();
                getAxis = type.GetMethod("GetAxis", new Type[] { typeof(string) });
            }

            return (float)getAxis.Invoke(player, new object[] { axisName });
        }

        public override bool GetButton(string buttonName) { return GetButton(buttonName, 0); }

        public override bool GetButtonDown(string buttonName) { return GetButtonDown(buttonName, 0); }

        public override bool GetButtonUp(string buttonName) { return GetButtonUp(buttonName, 0); }

        public override bool GetButton(string buttonName, int playerId)
        {
            object player = GetPlayer(playerId);
            if (getButton == null)
            {
                Type type = player.GetType();
                getButton = type.GetMethod("GetButton", new Type[] { typeof(string) });
            }

            return (bool)getButton.Invoke(player, new object[] { buttonName });
        }

        public override bool GetButtonDown(string buttonName, int playerId)
        {
            object player = GetPlayer(playerId);
            if (getButtonDown == null)
            {
                Type type = player.GetType();
                getButtonDown = type.GetMethod("GetButtonDown", new Type[] { typeof(string) });
            }

            return (bool)getButtonDown.Invoke(player, new object[] { buttonName });
        }

        public override bool GetButtonUp(string buttonName, int playerId)
        {
            object player = GetPlayer(playerId);
            if (getButtonUp == null)
            {
                Type type = player.GetType();
                getButtonUp = type.GetMethod("GetButtonUp", new Type[] { typeof(string) });
            }

            return (bool)getButtonUp.Invoke(player, new object[] { buttonName });
        }

        public override bool GetKey(KeyCode key) { return Input.GetKey(key); }

        public override bool GetKeyDown(KeyCode key) { return Input.GetKeyDown(key); }

        public override bool GetKeyUp(KeyCode key) { return Input.GetKeyUp(key); }

        #endregion

        #region Private Methods

        private object GetPlayer(int playerId)
        {
            if (asm == null) Startup();

            if (playerList[playerId] == null)
            {
                playerList[playerId] = getPlayer.Invoke(players, new object[] { playerId });

            }

            return playerList[playerId];
        }

        private void Startup()
        {
            asm = Assembly.Load("Rewired_Core");
            if (asm == null)
            {
                StringExtensions.LogWarning(name, "RewiredInput", "Rewired is not installed");
                return;
            }

            Type reInput = null;
            foreach (Type type in asm.GetTypes())
            {
                if (type.Name == "ReInput")
                {
                    reInput = type;
                    break;
                }
            }
            if (reInput == null)
            {
                StringExtensions.LogError(name, "RewiredInput", "Could not find ReInput class");
                return;
            }

            PropertyInfo playersProp = reInput.GetProperty("players", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            players = playersProp.GetValue(null);
        }

        #endregion

    }
}