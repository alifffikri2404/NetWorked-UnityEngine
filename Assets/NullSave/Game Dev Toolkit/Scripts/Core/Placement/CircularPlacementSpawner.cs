using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NullSave.GDTK
{
    [RequireComponent(typeof(CircularPlacement))]
    public class CircularPlacementSpawner : MonoBehaviour, IBroadcastReceiver
    {

        #region Fields

        [Tooltip("Determines if the spawn animates to its destination")] public bool animateSpawn;
        [Tooltip("Seconds it takes to move to destination")] public float duration;
        [Tooltip("Curve to apply to spawns Y axis durtion animation")] public AnimationCurve yCurve;
        [Tooltip("If true the prefabs position is taken into account, otherwise it is not")] public bool usePrefabOffset;

        public List<WeightedUnityEntry> entries;

        [Tooltip("")] public bool respondToBroadcasts;
        [Tooltip("What message value will cause us to destroy this object?")] public string spawnMessage;
        [Tooltip("Should we listen to public broadcasts for the message?")] public bool usePublicChannel;
        [Tooltip("What channel name will we listen for the message on?")] public string channelName;

        private CircularPlacement cp;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            cp = GetComponent<CircularPlacement>();
        }

        /// <summary>
        /// Remove subscriptions on death
        /// </summary>
        private void OnDestroy()
        {
            if (respondToBroadcasts)
            {
                Broadcaster.UnsubscribeFromAll(this);
            }
        }

        private void Reset()
        {
            duration = 0.5f;
            yCurve = new AnimationCurve();
            yCurve.AddKey(0, 0.9f);
            yCurve.AddKey(1, 0);
        }

        /// <summary>
        /// Subscribe to broadcasts based on settings
        /// </summary>
        private void Start()
        {
            if (respondToBroadcasts)
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
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Generate spawn based on weighted lists
        /// </summary>
        public void SpawnItems()
        {
            int total = 0;
            List<WeightedUnityItem> finalItems = new List<WeightedUnityItem>();

            // Generate list
            List<WeightedUnityItem> items = WeightTable.GeneratedWeightedResult(entries);

            // Pass 1: remove non-gameobjects and get count
            foreach (WeightedUnityItem item in items)
            {
                if (item.Object is GameObject)
                {
                    if (item.UseRandomCount) item.Count = Random.Range(item.MinCount, item.MaxCount);
                    total += item.Count;
                    finalItems.Add(item);
                }
                else
                {
                    StringExtensions.Log(name, "CircularPlacment", "Non-GameObject supplied, skipping");
                }
            }

            // Get available positions
            List<Vector3> positions = cp.GetRandomFreePositions(total);
            foreach (WeightedUnityItem item in finalItems)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    if (positions.Count == 0)
                    {
                        StringExtensions.Log(name, "CircularPlacment", "Ran out of available spots early, ending prematurely");
                        break;
                    }

                    GameObject go = InterfaceManager.ObjectManagement.InstantiateObject((GameObject)item.Object);
                    if(item.randomRotation)
                    {
                        go.transform.rotation = Quaternion.Euler(new Vector3(Random.Range(item.minRotation.x, item.maxRotation.x), Random.Range(item.minRotation.y, item.maxRotation.y), Random.Range(item.minRotation.z, item.maxRotation.z)));
                    }

                    if (animateSpawn)
                    {
                        if (usePrefabOffset)
                        {
                            StartCoroutine(AnimateObjectOut(go.transform, transform.position, positions[0] + go.transform.position));
                        }
                        else
                        {
                            StartCoroutine(AnimateObjectOut(go.transform, transform.position, positions[0]));
                        }
                    }
                    else
                    {
                        go.transform.position = positions[0];
                    }
                    go.SetActive(true);
                    positions.RemoveAt(0);
                }
            }
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
            if (channel == channelName && message == spawnMessage)
            {
                SpawnItems();
            }
        }

        /// <summary>
        /// Response to public broadcast
        /// </summary>
        /// <param name="message"></param>
        public void PublicBroadcastReceived(object sender, string message)
        {
            if (!usePublicChannel) return;
            if (message == spawnMessage)
            {
                SpawnItems();
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator AnimateObjectOut(Transform target, Vector3 startPos, Vector3 endPos)
        {
            float elapsed = 0;
            float percent;
            Vector3 nextPos;

            while (elapsed < duration)
            {
                percent = elapsed / duration;
                nextPos = Vector3.Lerp(startPos, endPos, percent);
                nextPos.y = yCurve.Evaluate(percent);
                target.position = nextPos;

                yield return new WaitForEndOfFrame();
                elapsed += Time.deltaTime;
            }
        }

        #endregion

    }
}