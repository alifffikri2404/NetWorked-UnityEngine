using System.Collections.Generic;
using UnityEngine;

namespace NullSave.GDTK
{
    public class CircularPlacement : MonoBehaviour
    {

        #region Fields

        [Tooltip("Should we draw Gizmos in the editor?")] public bool drawGizmos;
        [Tooltip("What is the radius of objects to place?")] public float placementRadius;
        [Tooltip("What offset should we apply during placement?")] public Vector3 placementOffset;
        [Tooltip("What layers should we use to check if a space is free?")] public LayerMask placementMask;
        public List<CircularPlacementRing> rings;

        #endregion

        #region Properties

        /// <summary>
        /// List of all locations
        /// </summary>
        public List<Vector3> AllRelativeLocations { get; private set; }

        /// <summary>
        /// List of locations claimed or reserved
        /// </summary>
        public List<Vector3> ClaimedRelativeLocations { get; set; }

        #endregion

        #region Unity Methods

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!drawGizmos) return;

            int procCount = 0;
            float degrees;
            Vector3 placement;

            Gizmos.color = Color.green;

            foreach (CircularPlacementRing ring in rings)
            {
                if (ring.degreesPerStep < 0.1f) ring.degreesPerStep = 0.1f;
                if (ring.distanceFromCenter < 0.1f) ring.distanceFromCenter = 0.1f;

                degrees = 0;
                while (degrees < 360)
                {
                    placement = Quaternion.Euler(0, degrees, 0) * (transform.forward * ring.distanceFromCenter) + placementOffset;
                    Gizmos.DrawWireSphere(transform.position + placement, placementRadius);

                    degrees += ring.degreesPerStep;
                    procCount += 1;
                    if (procCount > 150)
                    {
                        StringExtensions.Log(name, "CircularPlacment", "Too many gizmos, exiting for safey");
                        return;
                    }
                }
            }
        }
#endif

        private void Reset()
        {
            drawGizmos = true;
            placementMask = 1;
            placementRadius = 0.5f;
            rings = new List<CircularPlacementRing>();
            rings.Add(new CircularPlacementRing { distanceFromCenter = 1.25f, degreesPerStep = 51.5f });
            rings.Add(new CircularPlacementRing { distanceFromCenter = 2.4f, degreesPerStep = 25.75f });
            rings.Add(new CircularPlacementRing { distanceFromCenter = 3.6f, degreesPerStep = 18f });
            rings.Add(new CircularPlacementRing { distanceFromCenter = 4.7f, degreesPerStep = 13.86f });
        }

        private void Start()
        {
            float degrees;

            ClaimedRelativeLocations = new List<Vector3>();

            // Load locations
            AllRelativeLocations = new List<Vector3>();
            foreach (CircularPlacementRing ring in rings)
            {
                if (ring.degreesPerStep < 0.1f) ring.degreesPerStep = 0.1f;
                if (ring.distanceFromCenter < 0.1f) ring.distanceFromCenter = 0.1f;

                degrees = 0;
                while (degrees < 360)
                {
                    AllRelativeLocations.Add(Quaternion.Euler(0, degrees, 0) * (transform.forward * ring.distanceFromCenter) + placementOffset);
                    degrees += ring.degreesPerStep;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets first available position, returns Vector3.zero if not spots available
        /// </summary>
        /// <returns></returns>
        public Vector3 GetFirstFreePosition(bool excludeClaimed = false)
        {
            for (int i = 0; i < AllRelativeLocations.Count; i++)
            {
                if (!Physics.Raycast(transform.position + AllRelativeLocations[i] + new Vector3(0, 1, 0), -transform.up, 0.99f, placementMask))
                {
                    if (!excludeClaimed || !ClaimedRelativeLocations.Contains(AllRelativeLocations[i]))
                    {
                        return transform.position + AllRelativeLocations[i];
                    }
                }
            }

            return Vector3.zero;
        }

        /// <summary>
        /// Finds a random free position, returns Vector3.zero if not spots available
        /// </summary>
        /// <returns></returns>
        public Vector3 GetRandomFreePosition(bool excludeClaimed = false)
        {
            int index;
            List<Vector3> tempPos = new List<Vector3>();
            tempPos.AddRange(AllRelativeLocations);

            while (tempPos.Count > 0)
            {
                index = Random.Range(0, tempPos.Count);
                if (!Physics.Raycast(transform.position + tempPos[index] + new Vector3(0, 1, 0), -transform.up, 0.99f, placementMask))
                {
                    if (!excludeClaimed || !ClaimedRelativeLocations.Contains(AllRelativeLocations[index]))
                    {
                        return transform.position + tempPos[index];
                    }
                }
                else
                {
                    tempPos.RemoveAt(index);
                }
            }

            return Vector3.zero;
        }

        /// <summary>
        /// Finds a list of random free positions
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Vector3> GetRandomFreePositions(int count, bool excludeClaimed = false)
        {
            List<Vector3> result = new List<Vector3>();
            int index;
            List<Vector3> tempPos = new List<Vector3>();
            tempPos.AddRange(AllRelativeLocations);

            while (tempPos.Count > 0 && count > 0)
            {
                index = Random.Range(0, tempPos.Count);
                if (!Physics.Raycast(transform.position + tempPos[index] + new Vector3(0, 1, 0), -transform.up, 0.99f, placementMask))
                {
                    if (!excludeClaimed || !ClaimedRelativeLocations.Contains(AllRelativeLocations[index]))
                    {
                        result.Add(transform.position + tempPos[index]);
                        count -= 1;
                    }
                }

                tempPos.RemoveAt(index);
            }

            return result;
        }

        #endregion

    }
}