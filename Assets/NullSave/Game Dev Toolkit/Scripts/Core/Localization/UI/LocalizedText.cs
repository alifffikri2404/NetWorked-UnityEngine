using UnityEngine;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    [RequireComponent(typeof(Text))]
    public class LocalizedText : MonoBehaviour
    {

        #region Fields

        [Tooltip("Format to use for localizing text")] [TextArea(3, 10)] public string format = "[entry:entryId]";

        private Text target;

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
            target = GetComponent<Text>();

            Localize.onLanguageChanged.AddListener(LanguageChanged);
            if (Localize.Initialized)
            {
                LanguageChanged(Localize.CurrentLanguage);
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