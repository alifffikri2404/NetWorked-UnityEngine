using System;

namespace NullSave.GDTK
{
    [Serializable]
    public class WeightedItem
    {

        #region Fields

        public int Count;
        public object Object;
        public bool UseRandomCount;
        public int MinCount;
        public int MaxCount;

        #endregion

    }
}