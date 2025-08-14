using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    public class DataList : MonoBehaviour
    {

        #region Fields

        public UnityEvent onSelectionChanged;

        #endregion

        #region Properties

        public virtual int selectedIndex { get { throw new System.NotImplementedException(); } }

        public virtual string selectedKey { get { throw new System.NotImplementedException(); } }

        #endregion

        #region Public Methods

        public virtual void AddOption(string value) { throw new System.NotImplementedException(); }

        public virtual void AddOption(string key, string value) { throw new System.NotImplementedException(); }

        public virtual void AddOptions(List<string> options) { throw new System.NotImplementedException(); }

        public virtual void AddOptions(Dictionary<string, string> options) { throw new System.NotImplementedException(); }

        public virtual void Clear() { throw new System.NotImplementedException(); }

        public virtual int GetSelectedIndex() { throw new System.NotImplementedException(); }

        public virtual string GetSelectedItem() { throw new System.NotImplementedException(); }

        public virtual void SetSelection(string option) { throw new System.NotImplementedException(); }

        #endregion

    }
}