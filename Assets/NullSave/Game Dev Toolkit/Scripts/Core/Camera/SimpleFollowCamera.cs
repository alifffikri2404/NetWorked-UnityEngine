using UnityEngine;

namespace NullSave.GDTK
{
    public class SimpleFollowCamera : MonoBehaviour
    {

        #region Fields

        public Transform target;
        public Vector3 positionOffset;
        public Vector3 lookAtOffset;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if(target == null)
            {
                FindPlayer();
                if (!enabled) return;
            }
        }

        private void Update()
        {
            transform.position = target.transform.position + positionOffset;
            transform.LookAt(transform.position + lookAtOffset);
        }

        #endregion

        #region Private Methods

        private void FindPlayer()
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go != null)
            {
                target = go.gameObject.transform;
            }
            else
            {
                StringExtensions.Log(name, "Simple3rdPersonCamera", "No Player tagged object.");
                enabled = false;
            }
        }

        #endregion

    }
}