using System;
using UnityEngine;

namespace NullSave.GDTK
{
    [Serializable]
    public class FlexListOption
    {

        #region Properties

        public string text;

        public Sprite image;

        #endregion

        #region Constructors

        public FlexListOption() { }

        public FlexListOption(string text) { this.text = text; }

        public FlexListOption(Sprite image) { this.image = image; }

        public FlexListOption(string text, Sprite image) { this.text = text; this.image = image; }

        #endregion

    }
}