using System;
using UnityEngine;

namespace NullSave.GDTK
{
    [Serializable]
    public class UIToggleColorBlock
    {

        #region Fields

        public Color normalColor;
        public Color pressedColor;
        public Color highlightedColor;
        public Color disabledColor;
        [Range(1, 5)] public float colorMultiplier;
        public float fadeDuration;

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (obj is UIColorBlock comp)
            {
                if (normalColor != comp.normalColor) return false;
                if (highlightedColor != comp.highlightedColor) return false;
                if (pressedColor != comp.pressedColor) return false;
                if (disabledColor != comp.disabledColor) return false;
                if (colorMultiplier != comp.colorMultiplier) return false;
                if (fadeDuration != comp.fadeDuration) return false;

                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

    }
}