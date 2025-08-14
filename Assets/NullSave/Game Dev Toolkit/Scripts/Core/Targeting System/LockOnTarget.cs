using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDoc("This component allows the GameObject to be targeted by the `Target System`.")]
    public class LockOnTarget : MonoBehaviour
    {

        #region Fields

        [Tooltip("Offset to apply to spawned object")] public Vector3 indicatorOffset;

        private GameObject spawned;
        private bool locked;

        #endregion

        #region Public Methods

        [AutoDoc("Sets the lock-on indicator for this target.", "sampleObject.SetIndicator(myPrefab, true);")]
        [AutoDocParameter("GameObject to clone as indicator")]
        [AutoDocParameter("Is target currently locked on")]
        public void UpdateIndicator(GameObject go, bool isLocked)
        {
            if(go == null)
            {
                if(spawned != null)
                {
                    InterfaceManager.ObjectManagement.DestroyObject(spawned);
                }

                return;
            }

            if (isLocked == locked && spawned != null) return;

            if(spawned != null)
            {
                InterfaceManager.ObjectManagement.DestroyObject(spawned);
            }

            spawned = InterfaceManager.ObjectManagement.InstantiateObject(go, transform);
            spawned.transform.localPosition = indicatorOffset;
            locked = isLocked;
        }

        #endregion

    }
}