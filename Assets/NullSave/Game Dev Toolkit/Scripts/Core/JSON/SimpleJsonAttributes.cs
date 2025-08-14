using System;

namespace NullSave.GDTK
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class JsonSerialize : Attribute { }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class JsonDoNotSerialize : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public class JsonBeforeSerialization : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public class JsonAfterDeserialization : Attribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class JsonSerializeAs : Attribute
    {

        #region Fields

        public string SerializeName;

        #endregion

        #region Constructor

        public JsonSerializeAs(string serializeName)
        {
            SerializeName = serializeName;
        }

        #endregion

    }

}