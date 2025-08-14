using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    public class TriggerAfterTime : MonoBehaviour
    {

        #region Fields

        [Tooltip("Seconds to wait before triggering event")] public float secondsToWait;
        [Tooltip("Repeat event every X seconds")] public bool repeat;
        public UnityEvent onTimeElapsed;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            StartCoroutine(WaitAndInvoke());
        }

        #endregion

        #region Private Methods

        private IEnumerator WaitAndInvoke()
        {
            yield return new WaitForSecondsRealtime(secondsToWait);
            if (enabled)
            {
                onTimeElapsed?.Invoke();
                if(repeat)
                {
                    StartCoroutine(WaitAndInvoke());
                }
            }
        }

        #endregion

    }
}