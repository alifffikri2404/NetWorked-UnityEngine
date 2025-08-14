using System.Collections.Generic;
using UnityEngine;

namespace NullSave.GDTK
{
    [DefaultExecutionOrder(-170)]
    [RequireComponent(typeof(InlineList))]
    public class DataList_InlineList : DataList
    {

        #region Fields

        private InlineList target;
        private List<string> keys;

        #endregion

        #region Properties

        public override int selectedIndex
        {
            get { return target.selectedIndex; }
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            keys = new List<string>();
            target = GetComponent<InlineList>();
            target.onSelectionChanged.AddListener(() => onSelectionChanged?.Invoke());
        }

        #endregion

        #region Public Methods

        public override void AddOption(string value)
        {
            EnsureStartup();
            target.AddOption(value);
            keys.Add(value);
        }

        public override void AddOption(string key, string value)
        {
            EnsureStartup();
            target.AddOption(value);
            keys.Add(key);
        }

        public override void Clear()
        {
            EnsureStartup();
            target.Clear();
        }

        public override int GetSelectedIndex()
        {
            return target.selectedIndex;
        }

        public override string GetSelectedItem()
        {
            return target.selectedText;
        }

        public override void AddOptions(List<string> options)
        {
            EnsureStartup();
            target.AddOptions(options);
            keys.AddRange(options);
        }

        public override void AddOptions(Dictionary<string, string> options)
        {
            EnsureStartup();
            foreach (var entry in options)
            {
                target.AddOption(entry.Value);
                keys.Add(entry.Key);
            }
        }

        public override void SetSelection(string option)
        {
            EnsureStartup();
            for (int i = 0; i < target.options.Count; i++)
            {
                if(target.options[i] == option)
                {
                    target.selectedIndex = i;
                    return;
                }
            }
        }

        #endregion

        #region Private Methods

        private void EnsureStartup()
        {
            if (target == null)
            {
                target = GetComponent<InlineList>();
                keys = new List<string>();
            }
        }

        #endregion

    }
}