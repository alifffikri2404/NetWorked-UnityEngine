using System.Collections.Generic;
using static NullSave.GDTK.FlexList;

namespace NullSave.GDTK
{
    public class FlexListSortPlugin : SortAndFilterPlugin
    {

        #region Fields

        public bool enabled = true;
        public SortMode sortMode;

        private readonly string[] compat = new string[] { typeof(FlexListOption).ToString() };

        #endregion

        #region Properties

        public override string[] compatibleListTypes => compat;

        public override object sortBy
        {
            get { return sortMode; }
            set
            {
                if (value is SortMode useValue)
                {
                    sortMode = useValue;
                    requiresUpdate?.Invoke();
                }
            }
        }

        public override bool isEnabled => enabled;

        public override string title { get { return "FlexList Sort"; } }

        public override string description { get { return "Sort items in FlexList."; } }

        #endregion

        #region Public Methods

        public override void SortAndFilter<T>(List<T> list)
        {
            if (list is List<FlexListOption> typedList)
            {
                switch (sortMode)
                {
                    case SortMode.StringAsc:
                        typedList.Sort(new StringSortAsc());
                        break;
                    case SortMode.StringDesc:
                        typedList.Sort(new StringSortDesc());
                        break;
                    case SortMode.HasImageAsc:
                        typedList.Sort(new HasImageSortAsc());
                        break;
                    case SortMode.HasImageDesc:
                        typedList.Sort(new HasImageSortDesc());
                        break;
                }

            }

        }

        #endregion

        #region Comparer Classes

        private class HasImageSortAsc : IComparer<FlexListOption>
        {
            public int Compare(FlexListOption c1, FlexListOption c2) { return (c2.image != null).CompareTo(c1.image != null); }
        }

        private class HasImageSortDesc : IComparer<FlexListOption>
        {
            public int Compare(FlexListOption c1, FlexListOption c2) { return (c1.image != null).CompareTo(c2.image != null); }
        }

        private class StringSortAsc : IComparer<FlexListOption>
        {
            public int Compare(FlexListOption c1, FlexListOption c2) { return c2.text.CompareTo(c1.text); }
        }

        private class StringSortDesc : IComparer<FlexListOption>
        {
            public int Compare(FlexListOption c1, FlexListOption c2) { return c1.text.CompareTo(c2.text); }
        }

        #endregion


    }
}