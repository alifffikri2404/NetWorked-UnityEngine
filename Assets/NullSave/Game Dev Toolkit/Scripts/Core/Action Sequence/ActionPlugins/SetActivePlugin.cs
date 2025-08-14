using UnityEngine;

namespace NullSave.GDTK
{
    public class SetActivePlugin : ActionSequencePlugin
    {

        #region Fields

        public bool active;
        public bool useRemoteTarget;

        #endregion

        #region Properties

        public override Texture2D icon { get { return GetResourceImage("icons/check"); } }

        public override string title { get { return "Set Active"; } }

        public override string titlebarText
        {
            get
            {
                return "Set Active = " + active;
            }
        }

        public override string description { get { return "Set GameObject active state."; } }

        #endregion

        #region Plugin Methods

        public override void StartAction(ActionSequence host)
        {
            if (useRemoteTarget)
            {
                host.remoteTarget.SetActive(active);
            }
            else
            {
                host.gameObject.SetActive(active);
            }

            isComplete = true;
            isStarted = true;
        }

        #endregion

    }
}