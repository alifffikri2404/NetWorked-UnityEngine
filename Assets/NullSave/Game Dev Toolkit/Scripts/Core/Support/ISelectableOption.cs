using System;

namespace NullSave.GDTK
{
    public interface ISelectableOption
    {

        #region Properties

        public BasicInfo optionInfo { get; }

        public Type type { get; }

        #endregion

    }
}
