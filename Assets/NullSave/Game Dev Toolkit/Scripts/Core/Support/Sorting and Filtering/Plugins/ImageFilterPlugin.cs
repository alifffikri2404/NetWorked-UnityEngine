using System.Collections.Generic;
using UnityEngine;

namespace NullSave.GDTK
{
    public class ImageFilterPlugin : SortAndFilterPlugin
    {

        #region Fields

        public bool enabled = true;
        public bool requireImage = true;

        private readonly string[] compat = new string[] { typeof(FlexListOption).ToString() };

        #endregion

        #region Properties

        public override string[] compatibleListTypes => compat;

        public override object filterBy
        {
            get { return requireImage; }
            set
            {
                if (value is bool useValue)
                {
                    requireImage = useValue;
                    requiresUpdate?.Invoke();
                }
            }
        }

        public override bool isEnabled => enabled;

        public override Texture2D icon { get { return GetResourceImage("icons/filter"); } }

        public override string title { get { return "Image Filter"; } }

        public override string description { get { return "Filter results by image."; } }

        #endregion

        #region Public Methods

        public override void SortAndFilter<T>(List<T> list)
        {
            if (!requireImage) return;

            if (list is List<FlexListOption> flexList)
            {
                List<int> removeIndexList = new List<int>();

                for (int i = 0; i < list.Count; i++)
                {
                    if (flexList[i].image == null)
                    {
                        removeIndexList.Add(i);
                    }
                }

                // Remove last to first
                for (int i = removeIndexList.Count - 1; i >= 0; i--)
                {
                    list.RemoveAt(removeIndexList[i]);
                }
            }
        }

        #endregion

    }
}
