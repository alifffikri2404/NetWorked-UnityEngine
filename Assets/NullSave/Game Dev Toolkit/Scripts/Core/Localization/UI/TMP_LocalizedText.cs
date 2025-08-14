using TMPro;
using UnityEngine;

namespace NullSave.GDTK
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TMP_LocalizedText : MonoBehaviour
    {

        #region Fields

        [Tooltip("Format to use for localizing text")] [TextArea(3,10)] public string format = "[entry:entryId]";

        private TextMeshProUGUI target;

        #endregion

        #region Properties

        public string Text
        {
            get { return target.text; }
            set
            {
                target.text = Localize.GetFormattedString(value);
            }
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            target = GetComponent<TextMeshProUGUI>();

            Localize.onLanguageChanged.AddListener(LanguageChanged);
            if(Localize.Initialized)
            {
                LanguageChanged(Localize.CurrentLanguage);
            }
            else
            {
                Localize.Initialize();
            }
        }

        #endregion

        #region Private Methods

        private void LanguageChanged(string newLanguage)
        {
            target.text = Localize.GetFormattedString(format);
        }

        #endregion

    }
}