using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    [AutoDoc("This component raises events based on a layermask and collision events.")]
    public class TriggerByCollider3D : MonoBehaviour
    {

        #region Fields

        [Tooltip("Layermask used to determine trigger")] public LayerMask layerMask;
        [Tooltip("Event raised on collision enter")] public UnityEvent onEnter;
        [Tooltip("Event raised on collision exit")] public UnityEvent onExit;
        [Tooltip("Event raised on collision stay")] public UnityEvent onStay;

        #endregion

        #region Unity Methods

        private void OnCollisionEnter(Collision collision)
        {
            if (layerMask != (layerMask | (1 << collision.gameObject.layer))) return;
            onEnter?.Invoke();
        }

        private void OnCollisionExit(Collision collision)
        {
            if (layerMask != (layerMask | (1 << collision.gameObject.layer))) return;
            onExit?.Invoke();
        }

        private void OnCollisionStay(Collision collision)
        {
            if (layerMask != (layerMask | (1 << collision.gameObject.layer))) return;
            onStay?.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (layerMask != (layerMask | (1 << other.gameObject.layer))) return;
            onEnter?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (layerMask != (layerMask | (1 << other.gameObject.layer))) return;
            onExit?.Invoke();
        }

        private void OnTriggerStay(Collider other)
        {
            if (layerMask != (layerMask | (1 << other.gameObject.layer))) return;
            onStay?.Invoke();
        }

        private void Reset()
        {
            layerMask = 1;
        }

        #endregion

    }
}