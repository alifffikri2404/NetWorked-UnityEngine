using System;
using UnityEngine;

namespace NullSave.GDTK
{
    [Serializable]
    public class UIColorBlock
    {

        #region Fields

        public Color normalColor;
        public Color selectedColor;
        public Color highlightedColor;
        public Color pressedColor;
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
                if (selectedColor != comp.selectedColor) return false;
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