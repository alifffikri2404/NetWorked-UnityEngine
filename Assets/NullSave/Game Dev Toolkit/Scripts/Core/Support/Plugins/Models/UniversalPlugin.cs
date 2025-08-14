using System;
using UnityEngine;

namespace NullSave.GDTK
{
    [Serializable]
    public class UniversalPlugin
    {

        #region Properties

        public virtual Texture2D icon { get { return GetResourceImage(null); } }

        public virtual string title { get { return "Untitled Plugin"; } }

        public virtual string titlebarText { get { return title; } }

        public virtual string description { get { return "This plugin has not been fully completed."; } }

        #endregion

        #region Protected Methods

        protected Texture2D GetResourceImage(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return (Texture2D)Resources.Load("icons/plugin", typeof(Texture2D));
            }
            return (Texture2D)Resources.Load(path, typeof(Texture2D));
        }

        #endregion

    }
}
