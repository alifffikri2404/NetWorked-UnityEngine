using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    public class TinyBroadcastReceiver : MonoBehaviour, IBroadcastReceiver
    {

        #region Fields

        [Tooltip("Use public channel if checked, otherwise use named channel")] public bool usePublicChannel;
        [Tooltip("Name of channel to subscribe to")] public string channelName;

        [Tooltip("Message to wait for")] public string awaitMessage;

        public UnityEvent onMessageReceived;

        #endregion

        #region Unity Events

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
            if (channel == channelName && message == awaitMessage)
            {
                onMessageReceived?.Invoke();
            }
        }

        /// <summary>
        /// Response to public broadcast
        /// </summary>
        /// <param name="message"></param>
        public void PublicBroadcastReceived(object sender, string message)
        {
            if (!usePublicChannel) return;
            if (message == awaitMessage)
            {
                onMessageReceived?.Invoke();
            }
        }

        #endregion

    }
}