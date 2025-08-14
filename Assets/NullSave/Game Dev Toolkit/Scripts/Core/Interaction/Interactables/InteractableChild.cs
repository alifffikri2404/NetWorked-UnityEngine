using UnityEngine;

namespace NullSave.GDTK
{
    [RequireComponent(typeof(Collider))]
    public class InteractableChild : MonoBehaviour
    {

        #region Fields

        [Tooltip("Parent interactable obejct")] public InteractableObject parentInteractable;

        #endregion

        #region Properties

        public OcclusionPortal OcclusionPortal { get; private set; }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            OcclusionPortal = GetComponent<OcclusionPortal>();
        }

        #endregion

    }
}