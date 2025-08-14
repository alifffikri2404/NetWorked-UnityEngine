using UnityEngine;

namespace NullSave.GDTK
{
    public class SetComponentValuePlugin : ActionSequencePlugin
    {

        #region Fields

        public bool useRemoteTarget;
        public string componentType;
        public string memberName;
        public string memberType;
        public string memberValue;
        public string memberJson;

        #endregion

        #region Properties

        public override Texture2D icon { get { return GetResourceImage("icons/wizard"); } }

        public override string title { get { return "Set Component Value"; } }

        public override string description { get { return "Get a component on attached GameObject and set a value."; } }

        #endregion

        #region Public Methods

        public override void StartAction(ActionSequence host)
        {
            isStarted = true;

            // Find object
            object component = GetComponent(useRemoteTarget ? host.remoteTarget : host.gameObject);
            if (component == null)
            {
                StringExtensions.Log(host.name, "SetComponentValuePlugin", "Could not find component of type " + componentType);
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