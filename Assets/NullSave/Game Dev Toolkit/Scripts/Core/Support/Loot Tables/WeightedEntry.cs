using System;
using System.Collections.Generic;

namespace NullSave.GDTK
{
    [Serializable]
    public class WeightedEntry
    {

        #region Fields

        public float Weight;
        public List<WeightedItem> Items;

        #endregion

    }
}