using System;
using System.Reflection;
using UnityEngine;

namespace NullSave.GDTK
{
    public class SetGameObjectComponentValuePlugin : ActionSequencePlugin
    {

        #region Fields

        public string gameObjectName;
        public string componentType;
        public string memberName;
        public string memberType;
        public string memberValue;
        public string memberJson;

        #endregion

        #region Properties

        public override Texture2D icon { get { return GetResourceImage("icons/wizard"); } }

        public override string title { get { return "Set GameObject Component Value"; } }

        public override string description { get { return "Find a game object and then a component attached to it and set a value."; } }

        #endregion

        #region Public Methods

        public override void StartAction(ActionSequence host)
        {
            isStarted = true;

            // Find GameObject
            GameObject go = GameObject.Find(gameObjectName);
            if(go == null)
            {
                StringExtensions.Log(host.name, "SetGameObjectComponentValuePlugin", "Could not find GameObject named " + gameObjectName);
                isComplete = true;
                return;
            }

            // Find object
            object component = GetComponent(go);
            if (component == null)
            {
                StringExtensions.Log(host.name, "SetGameObjectComponentValuePlugin", "Could not find component of type " + componentType);
                isComplete = true;
                return;
            }

            ActionSequenceHelper.SetMemberValue(component, memberName, memberValue, memberJson);

            isComplete = true;
        }

        #endregion

        #region Private Methods

        private object GetComponent(GameObject host)
        {
            object component = host.GetComponent(componentType);
            if (component != null) return component;
            try
            {
                component = host.GetComponent(componentType.Substring(componentType.LastIndexOf('.') + 1));
            }
            catch { }
            return component;
        }

        #endregion

    }
}