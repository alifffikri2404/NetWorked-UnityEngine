using System;
using UnityEngine;

namespace NullSave.GDTK
{
    [Serializable]
    public class TemplatedLabel
    {

        #region Fields

        [Tooltip("Label to target when setting formatted text")] public Label target;
        [Tooltip("Format to apply to text")] [TextArea(2, 5)] public string format;

        #endregion

    }
}
