using System;
using UnityEngine;

namespace NullSave.GDTK
{

    [Serializable]
    public struct UISpriteState
    {
        public Sprite selectedSprite;
        public Sprite highlightedSprite;
        public Sprite pressedSprite;
        public Sprite disabledSprite;
    }

    [Serializable]
    public struct UIToggleSpriteState
    {
        public Sprite highlightedSprite;
        public Sprite pressedSprite;
        public Sprite disabledSprite;
    }

}