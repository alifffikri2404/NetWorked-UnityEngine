using UnityEngine;

namespace NullSave.GDTK
{
    public class RotateToMainCamera : MonoBehaviour
    {

        #region Fields

        public Vector3 offset;

        #endregion

        #region Unity Methods

        private void Update()
        {
            transform.LookAt(Camera.main.transform.position);
            transform.Rotate(offset);
        }

        #endregion

    }
}