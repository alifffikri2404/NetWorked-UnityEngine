using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDoc("This component allows the attached GameObject to be targeted by an Object Pull Source.")]
    public class ObjectPullTarget : MonoBehaviour
    {

        #region Fields

        [Tooltip("Additional seconds to delay before pull starts")] public float additionalDelay;
        [Tooltip("Additional seconds to add to pull duration")] public float additionalDuration;

        #endregion

        #region Properties

        [AutoDoc("Seconds of delay already elapsed.", "sampleObject.ElapsedDelay = 0;")]
        public float ElapsedDelay { get; set; }

        [AutoDoc("Seconds spent pulling object.", "sampleObject.ElapsedPull = 0;")]
        public float ElapsedPull { get; set; }

        [AutoDoc("Position of object before pull started.", "sampleObject.StartPosition = new Vector3(0, 0, 0);")]
        public Vector3 StartPosition { get; set; }

        #endregion

    }
}