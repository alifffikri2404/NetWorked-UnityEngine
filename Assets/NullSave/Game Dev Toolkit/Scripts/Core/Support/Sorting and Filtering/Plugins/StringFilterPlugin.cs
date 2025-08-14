using System.Collections.Generic;
using UnityEngine;

namespace NullSave.GDTK
{
    public class StringFilterPlugin : SortAndFilterPlugin
    {

        #region Fields

        public bool enabled = true;
        public string filter;

        private readonly string[] compat = new string[] { typeof(string).ToString(), typeof(FlexListOption).ToString() };

        #endregion

        #region Properties

        public override string[] compatibleListTypes => compat;

        public override object filterBy
        {
            get { return filter; }
            set
            {
                if(value is string stringValue)
                {
                    filter = stringValue;
                    requiresUpdate?.Invoke();
                }
            }
        }

        public override bool isEnabled => enabled;

        public override Texture2D icon { get { return GetResourceImage("icons/filter"); } }

        public override string title { get { return "String Filter"; } }

        public override string description { get { return "Filter string results."; } }

        #endregion

        #region Public Methods

        public override void SortAndFilter<T>(List<T> list)
        {
            if (string.IsNullOrEmpty(filter)) return;

            List<int> removeIndexList = new List<int>();

            if (list is List<string> stringList)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (stringList[i].IndexOf(filter, System.StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        removeIndexList.Add(i);
                    }
                }
            }
            else if (list is List<FlexListOption> flexList)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (flexList[i].text.IndexOf(filter, System.StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        removeIndexList.Add(i);
                    }
                }
            }

            // Remove last to first
            for(int i = removeIndexList.Count -1; i >= 0; i--)
            {
                list.RemoveAt(removeIndexList[i]);
            }
        }

        #endregion

    }
}