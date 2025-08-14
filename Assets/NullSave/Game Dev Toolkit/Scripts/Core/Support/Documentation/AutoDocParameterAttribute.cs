using System;

namespace NullSave.GDTK
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AutoDocParameter : Attribute
    {

        #region Fields

        public string Description;

        #endregion

        #region Constructor

        public AutoDocParameter(string description)
        {
            Description = description;
        }

        #endregion


    }
}
