using UnityEngine;

namespace NullSave.GDTK
{
    public class PublicBroadcastPlugin : ActionSequencePlugin
    {

        #region Fields

        public string message;

        #endregion

        #region Properties

        public override Texture2D icon { get { return GetResourceImage("icons/broadcast"); } }

        public override string title { get { return "Public Broadcast"; } }

        public override string description { get { return "Sends a message via Broadcaster on the public channel."; } }

        #endregion

        #region Plugin Methods

        public override void StartAction(ActionSequence host)
        {
            isComplete = false;
            isStarted = true;
            Broadcaster.PublicBroadcast(host.gameObject, message);
            isComplete = true;
        }

        #endregion

    }
}