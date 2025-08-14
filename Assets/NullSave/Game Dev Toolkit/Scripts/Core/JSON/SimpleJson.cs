using System;

namespace NullSave.GDTK
{
    public static class SimpleJson
    {

        #region Public Methods

        public static T FromJSON<T>(string json)
        {
            JsonDeserializationObject jdo = new JsonDeserializationObject(json);
            return jdo.Deserialize<T>();
        }

        public static object FromJSON(string json, Type type)
        {
            JsonDeserializationObject jdo = new JsonDeserializationObject(json);
            return jdo.Deserialize(type);
        }

        /// <summary>
        /// Simple method for serializing to JSON.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="serializationType"></param>
        /// <returns></returns>
        public static string ToJSON(object obj, bool removeNulls = true, bool readable = false)
        {
            return new JsonSerializationObject(obj, removeNulls, readable).value;
        }

        #endregion

    }
}