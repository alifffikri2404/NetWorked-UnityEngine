using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDoc("This component waits for a specific message/channel combination to be received and then destroys the attached GameObject.")]
    public class DestroyOnBroadcastMessage : MonoBehaviour, IBroadcastReceiver
    {

        #region Fields

        [Tooltip("What message value will cause us to destroy this object?")] public string destroyMessage;
        [Tooltip("Should we listen to public broadcasts for the message?")] public bool usePublicChannel;
        [Tooltip("What channel name will we listen for the message on?")] public string channelName;

        #endregion

        #region Unity Methods

        /// <summary>
        /// Remove subscriptions on death
        /// </summary>
        private void OnDestroy()
        {
            Broadcaster.UnsubscribeFromAll(this);
        }

        /// <summary>
        /// Subscribe to broadcasts based on settings
        /// </summary>
        private void Start()
        {
            if (usePublicChannel)
            {
                Broadcaster.SubscribeToPublic(this);
            }
            else
            {
                Broadcaster.SubscribeToChannel(this, channelName);
            }
        }

        private void Reset()
        {
            destroyMessage = "die";
            usePublicChannel = true;
        }

        #endregion

        #region Receiver Methods

        /// <summary>
        /// Response to channel broadcast
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void BroadcastReceived(object sender, string channel, string message, object[] args)
        {
            if (usePublicChannel) return;
            if (channel == channelName && message == destroyMessage)
            {
                InterfaceManager.ObjectManagement.DestroyObject(gameObject);
            }
        }

        /// <summary>
        /// Response to public broadcast
        /// </summary>
        /// <param name="message"></param>
        public void PublicBroadcastReceived(object sender, string message)
        {
            if (!usePublicChannel) return;
            if (message == destroyMessage)
            {
                InterfaceManager.ObjectManagement.DestroyObject(gameObject);
            }
        }

        #endregion

    }
}