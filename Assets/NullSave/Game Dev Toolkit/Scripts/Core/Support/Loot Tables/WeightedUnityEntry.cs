using System;
using System.Collections.Generic;

namespace NullSave.GDTK
{
    [Serializable]
    public class WeightedUnityEntry
    {

        #region Fields

        public float Weight;
        public List<WeightedUnityItem> Items;

        #endregion

    }
}