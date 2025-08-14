using UnityEngine;

namespace NullSave.GDTK
{
    public class InvokeComponentMethodPlugin : ActionSequencePlugin
    {

        #region Fields

        public bool useRemoteTarget;
        public string componentType;
        public string methodName;
        public string methodSignature;
        public string parameterJson;

        #endregion

        #region Properties

        public override Texture2D icon { get { return GetResourceImage("icons/wizard"); } }

        public override string title { get { return "Invoke Component Method"; } }

        public override string description { get { return "Get a component on attached GameObject and invoke a method."; } }

        #endregion

        #region Public Methods

        public override void StartAction(ActionSequence host)
        {
            isStarted = true;

            // Find object
            object component = GetComponent(useRemoteTarget ? host.remoteTarget : host.gameObject);
            if (component == null)
            {
                StringExtensions.Log(host.name, "InvokeComponentMethodPlugin", "Could not find component of type " + componentType);
                isComplete = true;
                return;
            }

            ActionSequenceHelper.InvokeMatchingSignature(component, methodName, methodSignature, parameterJson);

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