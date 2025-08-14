using System;
using System.Collections.Generic;

namespace NullSave.GDTK.JSON
{
    [Serializable]
    public class jsonActionSequenceList
    {

        #region Fields

        public string id;
        public List<jsonUniversalPlugin> actions;

        #endregion

        #region Constructor

        public jsonActionSequenceList()
        {
            actions = new List<jsonUniversalPlugin>();
        }

        #endregion

    }
}
