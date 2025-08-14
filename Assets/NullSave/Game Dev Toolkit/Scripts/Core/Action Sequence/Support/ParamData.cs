using System;

namespace NullSave.GDTK
{
    [Serializable]
    public class ParamData
    {

        #region Fields

        public string paramName;
        public string paramType;
        public string paramValue;
        public string paramJson;

        #endregion

        #region Propeties

        public Type type { get; set; }

        public object value { get; set; }

#if UNITY_EDITOR

        public string objectError { get; set; }

#endif

        #endregion

    }
}
