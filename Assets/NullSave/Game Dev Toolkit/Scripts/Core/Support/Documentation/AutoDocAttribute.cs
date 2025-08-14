using System;

namespace NullSave.GDTK
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method)]
    public class AutoDoc : Attribute
    {

        #region Fields

        public string Description;
        public string Usage;

        #endregion

        #region Constructor

        public AutoDoc(string description)
        {
            Description = description;
        }

        public AutoDoc(string description, string usage)
        {
            Description = description;
            Usage = usage;
        }

        #endregion

    }
}
