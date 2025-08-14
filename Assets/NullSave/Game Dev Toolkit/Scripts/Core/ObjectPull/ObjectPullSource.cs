using System.Collections.Generic;
using UnityEngine;

namespace NullSave.GDTK
{
    public class ObjectPullSource : MonoBehaviour
    {

        #region Fields

        [Tooltip("Layermask for setting included pull layers")] public LayerMask affectedLayers;
        [Tooltip("Seconds to way before beginning pull")] public float delayBeforePull;
        [Tooltip("Radius around object to pull from")] public float pullRadius;
        [Tooltip("Seconds from start of pull until object reaches target")] public float pullDuration;
        [Tooltip("Offset to apply to end position")] public Vector3 pullToOffset;
        [Tooltip("Destroy pulled target after pull complete")] public bool destroyAfterPull;

        private List<ObjectPullTarget> targets;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            targets = new List<ObjectPullTarget>();
        }

        private void FixedUpdate()
        {
            if (pullRadius < 0) pullRadius = 0;

            // Update hits
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, pullRadius, transform.up, affectedLayers);
            foreach (RaycastHit hit in hits)
            {
                ObjectPullTarget opt = hit.transform.gameObject.GetComponent<ObjectPullTarget>();
                if (opt != null && !targets.Contains(opt))
                {
                    opt.ElapsedDelay = 0;
                    opt.ElapsedPull = 0;
                    targets.Add(opt);
                }
            }

            // Cleanup Targets
            List<ObjectPullTarget> toRemove = new List<ObjectPullTarget>();
            foreach (ObjectPullTarget target in targets)
            {
                if (target == null || target.gameObject == null)
                {
                    toRemove.Add(target);
                }
            }
            foreach (ObjectPullTarget target in toRemove) targets.Remove(target);

            // Update pull
            foreach (ObjectPullTarget target in targets)
            {
                if (target.ElapsedDelay < delayBeforePull + target.additionalDelay)
                {
                    target.ElapsedDelay += Time.fixedDeltaTime;
                    if (target.ElapsedDelay >= delayBeforePull + target.additionalDelay)
                    {
                        target.StartPosition = target.transform.position;
                        target.ElapsedPull = delayBeforePull + target.additionalDelay - target.ElapsedDelay;
                    }
                }

                if (target.ElapsedDelay >= delayBeforePull + target.additionalDelay)
                {
                    target.ElapsedPull += Time.fixedDeltaTime;
                    target.transform.position = Vector3.Slerp(target.StartPosition, transform.position + pullToOffset, target.ElapsedPull / (pullDuration + target.additionalDuration));
                }

                if (target.ElapsedPull >= pullDuration + target.additionalDuration)
                {
                    if (destroyAfterPull)
                    {
                        InterfaceManager.ObjectManagement.DestroyObject(target.gameObject);
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (pullRadius < 0) pullRadius = 0;

            Gizmos.DrawWireSphere(transform.position, pullRadius);    
        }

        private void Reset()
        {
            affectedLayers = 1;
            pullRadius = 10;
            pullDuration = 1;
            delayBeforePull = 3;
        }

        #endregion

    }
}