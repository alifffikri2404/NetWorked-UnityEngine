using UnityEngine;

namespace NullSave.GDTK
{
    public class PrivateBroadcastPlugin : ActionSequencePlugin
    {

        #region Fields

        public string channelName;
        public string message;

        #endregion

        #region Properties

        public override Texture2D icon { get { return GetResourceImage("icons/broadcast"); } }

        public override string title { get { return "Private Broadcast"; } }

        public override string titlebarText
        {
            get
            {
                return "Broadcast on channel " + channelName;
            }
        }

        public override string description { get { return "Sends a message via Broadcaster on a private named channel."; } }

        #endregion

        #region Plugin Methods

        public override void StartAction(ActionSequence host)
        {
            isComplete = false;
            isStarted = true;
            Broadcaster.Broadcast(host.gameObject, channelName, message);
            isComplete = true;
        }

        #endregion

    }
}