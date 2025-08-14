using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    public class LanguageSelectedTrigger : MonoBehaviour
    {

        #region Fields

        [Tooltip("Language to wait for")] public string language;
        public UnityEvent onMatch, onNoMatch;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            Localize.onLanguageChanged.AddListener(LanguageChanged);
            LanguageChanged(Localize.CurrentLanguage);
        }

        #endregion

        #region Private Methods

        private void LanguageChanged(string newLanguage)
        {
            if (newLanguage == language)
            {
                onMatch?.Invoke();
            }
            else
            {
                onNoMatch?.Invoke();
            }
        }

        #endregion

    }
}