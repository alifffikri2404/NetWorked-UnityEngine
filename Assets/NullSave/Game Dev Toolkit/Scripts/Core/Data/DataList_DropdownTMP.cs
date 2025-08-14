using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NullSave.GDTK
{
    [DefaultExecutionOrder(-170)]
    [RequireComponent(typeof(TMP_Dropdown))]
    public class DataList_DropdownTMP : DataList
    {

        #region Fields

        private TMP_Dropdown target;
        private List<string> keys;

        #endregion

        #region Properties

        public override int selectedIndex
        {
            get { return target.value; }
        }

        public override string selectedKey
        {
            get
            {
                if (target.value >= keys.Count) return string.Empty;
                return keys[target.value];
            }
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            keys = new List<string>();
            target = GetComponent<TMP_Dropdown>();
            target.onValueChanged.AddListener((int value) => onSelectionChanged?.Invoke());
        }

        #endregion

        #region Public Methods

        public override void AddOption(string value)
        {
            EnsureStartup();
            target.options.Add(new TMP_Dropdown.OptionData { text = value });
            keys.Add(value);
        }

        public override void AddOption(string key, string value)
        {
            EnsureStartup();
            target.options.Add(new TMP_Dropdown.OptionData { text = value });
            keys.Add(key);
        }

        public override void Clear()
        {
            EnsureStartup();
            target.ClearOptions();
        }

        public override int GetSelectedIndex()
        {
            return target.value;
        }

        public override string GetSelectedItem()
        {
            return target.options[target.value].text;
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
                target.options.Add(new TMP_Dropdown.OptionData { text = entry.Value });
                keys.Add(entry.Key);
            }
        }

        public override void SetSelection(string option)
        {
            EnsureStartup();
            for (int i = 0; i < target.options.Count; i++)
            {
                if (target.options[i].text == option)
                {
                    target.value = i;
                    return;
                }
            }
        }

        #endregion

        #region Private Methods

        private void EnsureStartup()
        {
            if(target == null)
            {
                target = GetComponent<TMP_Dropdown>();
                keys = new List<string>();
            }
        }

        #endregion

    }
}