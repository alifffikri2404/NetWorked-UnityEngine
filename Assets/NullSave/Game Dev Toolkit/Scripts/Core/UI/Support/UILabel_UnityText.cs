using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    [RequireComponent(typeof(Text))]
    public class UILabel_UnityText : UILabel
    {

        #region Fields

        [SerializeField, TextArea(2, 6)] private string m_Text;
        public bool localize;

        public UnityEvent onResized;

        public Text target;

        #endregion

        #region Properties

        public override Color color
        {
            get { return target.color; }
            set { target.color = value; }
        }

        public override string text
        {
            get { return target.text; }
            set
            {
                if (m_Text == value) return;
                m_Text = value;

                if (localize)
                {
                    target.text = Localize.GetFormattedString(value);
                    onTextChanged?.Invoke();
                }
                else
                {
                    target.text = m_Text;
                    onTextChanged?.Invoke();
                }
            }
        }

        public Text unityText
        {
            get { return target; }
        }

        public string unlocalizedText
        {
            get { return m_Text; }
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
            else
            {
                Localize.Initialize();
            }
        }

        private void Reset()
        {
            target = GetComponent<Text>();
        }

        #endregion

        #region Private Methods

        private void LanguageChanged(string newLanguage)
        {
            if (!localize) return;

            target.text = Localize.GetFormattedString(m_Text);
        }

        #endregion

    }
}