using System;
using TMPro;

namespace NullSave.GDTK
{
    [Serializable]
    public class InputMap
    {

        #region Variables

        public string inputName;
        public int tmpSpriteIndex;

        #endregion

        #region Properties

        public TMP_SpriteAsset TMPSpriteAsset { get; set; }

        #endregion

    }
}