using UnityEngine;

namespace NullSave.GDTK
{
    public interface ITabStop
    {

        #region Fields

        GameObject attachedObject { get; }

        int parentStopId { get; set; }

        int tabStopId { get; set; }

        #endregion

    }
}