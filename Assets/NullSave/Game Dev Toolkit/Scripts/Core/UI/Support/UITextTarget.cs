using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    public class UILabel : MonoBehaviour
    {

        #region Fields

        public UnityEvent onTextChanged;

        #endregion

        #region Properties

        public virtual string text { get; set; }

        public virtual Color color { get; set; }

        #endregion

    }
}