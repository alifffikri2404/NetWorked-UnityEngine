using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NullSave.GDTK
{
    [ExecuteInEditMode]
    [DefaultExecutionOrder(-900)]
    public class Broadcaster : MonoBehaviour
    {

        #region Fields

        private List<BroadcastSubscription> channelReceivers;
        private List<IBroadcastReceiver> publicReceivers;
        private List<IBroadcastReceiver> globalReceivers;

        private static string preventRebirth;

        #endregion

        #region Properties

        /// <summary>
        /// Returns existing broadcaster if present, otherwise creates a new broadcaster and returns that
        /// </summary>
        /// <returns></returns>
        public static Broadcaster Current
        {
            get
            {
                Broadcaster result = ToolRegistry.GetComponent<Broadcaster>();
                if (result != null) return result;

                if (preventRebirth == SceneManager.GetActiveScene().name) return null;

                GameObject go = new GameObject("GDTK Broadcaster");
                result = go.AddComponent<Broadcaster>();
                DontDestroyOnLoad(go);

                return result;
            }
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            channelReceivers = new List<BroadcastSubscription>();
            publicReceivers = new List<IBroadcastReceiver>();
            globalReceivers = new List<IBroadcastReceiver>();
            preventRebirth = string.Empty;
        }

        private void OnDestroy()
        {
            preventRebirth = SceneManager.GetActiveScene().name;
        }

        private void OnDisable()
        {
            ToolRegistry.RemoveComponent(this);
        }

        private void OnEnable()
        {
            ToolRegistry.RegisterComponent(this);

            if (ToolRegistry.GetComponents<Broadcaster>().Count > 1)
            {
                StringExtensions.LogWarning(name, "Broadcaster", "More than one broadcaster is currently registered. Most components will only subscribe to the first one.");
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Broadcast a message to all receivers of a channel
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        public static void Broadcast(string channel, string message)
        {
            Broadcast(null, channel, message, null);
        }

        /// <summary>
        /// Broadcast a message to all receivers of a channel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        public static void Broadcast(object sender, string channel, string message)
        {
            Broadcast(sender, channel, message, null);
        }

        /// <summary>
        /// Broadcast a message to all receivers of a channel
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Broadcast(string channel, string message, object[] args)
        {
            Broadcast(null, channel, message, args);
        }

        /// <summary>
        /// Broadcast a message to all receivers of a channel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Broadcast(object sender, string channel, string message, object[] args)
        {
            foreach (BroadcastSubscription subscription in Current.channelReceivers)
            {
                if (subscription.channel == channel)
                {
                    subscription.receiver.BroadcastReceived(sender, channel, message, args);
                }
            }

            foreach (IBroadcastReceiver receiver in Current.globalReceivers)
            {
                receiver.BroadcastReceived(sender, channel, message, args);
            }
        }

        /// <summary>
        /// Broadcast to all receivers
        /// </summary>
        /// <param name="message"></param>
        public static void PublicBroadcast(object sender, string message)
        {
            // .ToArray() prevents collection modification collisions
            foreach (IBroadcastReceiver receiver in Current.publicReceivers.ToArray())
            {
                receiver.PublicBroadcastReceived(sender, message);
            }

            foreach (IBroadcastReceiver receiver in Current.globalReceivers.ToArray())
            {
                receiver.PublicBroadcastReceived(sender, message);
            }
        }

        /// <summary>
        /// Non-static version of PublicBroadcast so Unity can see/address
        /// </summary>
        /// <param name="message"></param>
        public void SendPublicBroadcast(string message)
        {
            PublicBroadcast(this, message);
        }

        /// <summary>
        /// Subscribe to all public and named channels
        /// </summary>
        /// <param name="receiver"></param>
        public static void SubscribeToAll(IBroadcastReceiver receiver)
        {
            Current.globalReceivers.Add(receiver);
        }

        /// <summary>
        /// Subscribe to a channel
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="channel"></param>
        public static void SubscribeToChannel(IBroadcastReceiver receiver, string channel)
        {
            BroadcastSubscription sub = new BroadcastSubscription { channel = channel, receiver = receiver };
            if (!Current.channelReceivers.Contains(sub))
            {
                Current.channelReceivers.Add(sub);
            }
        }

        /// <summary>
        /// Subscribe to public broadcasts
        /// </summary>
        /// <param name="receiver"></param>
        public static void SubscribeToPublic(IBroadcastReceiver receiver)
        {
            if (!Current.publicReceivers.Contains(receiver))
            {
                Current.publicReceivers.Add(receiver);
            }
        }

        /// <summary>
        /// Unsubscribe from all channels and public broadcasts
        /// </summary>
        /// <param name="receiver"></param>
        public static void UnsubscribeFromAll(IBroadcastReceiver receiver)
        {
            if (Current == null) return;
            Current.publicReceivers.Remove(receiver);
            Current.globalReceivers.Remove(receiver);

            List<BroadcastSubscription> removals = new List<BroadcastSubscription>();
            foreach (BroadcastSubscription subscription in Current.channelReceivers)
            {
                if (subscription.receiver == receiver)
                {
                    removals.Add(subscription);
                }
            }

            foreach (BroadcastSubscription subscription in removals)
            {
                Current.channelReceivers.Remove(subscription);
            }
        }

        /// <summary>
        /// Unsubscribe from a channel
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="channel"></param>
        public static void UnsubscribeFromChannel(IBroadcastReceiver receiver, string channel)
        {
            if (Current == null) return;
            List<BroadcastSubscription> removals = new List<BroadcastSubscription>();
            foreach (BroadcastSubscription subscription in Current.channelReceivers)
            {
                if (subscription.receiver == receiver && subscription.channel == channel)
                {
                    removals.Add(subscription);
                }
            }

            foreach (BroadcastSubscription subscription in removals)
            {
                Current.channelReceivers.Remove(subscription);
            }
        }

        /// <summary>
        /// Unsubscribe from public broadcasts
        /// </summary>
        /// <param name="receiver"></param>
        public static void UnsubscribeFromPublic(IBroadcastReceiver receiver)
        {
            if (Current == null) return;
            Current.publicReceivers.Remove(receiver);
        }

        #endregion

    }
}