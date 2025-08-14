using NullSave.GDTK.JSON;
using System;
using System.IO;
using UnityEngine;

namespace NullSave.GDTK
{
    [Serializable]
    public class UniversalPluginWrapper<T> : ISerializationCallbackReceiver where T : UniversalPlugin
    {

        #region Fields

        [JsonDoNotSerialize] [NonSerialized] public T plugin;
        public string serializationType;
        public string serializationData;
        public string serializationNamespace;

        [SerializeField] private string id;

#if UNITY_EDITOR
        [SerializeField] private bool z_expanded;
#endif

        #endregion

        #region Properties

        public string instanceId
        {
            get
            {
                if (string.IsNullOrEmpty(id))
                {
                    id = Guid.NewGuid().ToString();
                }
                return id;
            }
        }

        #endregion

        #region Constructors

        public UniversalPluginWrapper() { }

        public UniversalPluginWrapper(T plugin)
        {
            this.plugin = plugin;
            Type t = plugin.GetType();
            serializationData = SimpleJson.ToJSON(plugin);
            serializationType = t.ToString();
            serializationNamespace = t.Assembly.GetName().Name;
        }

        public UniversalPluginWrapper(T plugin, string data)
        {
            this.plugin = plugin;
            serializationData = data;
            Type t = plugin.GetType();
            serializationType = t.ToString();
            serializationNamespace = t.Assembly.GetName().Name;
        }

        public UniversalPluginWrapper(string id, string jsonNamespace, string jsonType, string jsonData)
        {
            this.id = id;
            serializationData = jsonData;
            serializationNamespace = jsonNamespace;
            serializationType = jsonType;

            try
            {
                if (string.IsNullOrEmpty(serializationNamespace))
                {
                    plugin = (T)SimpleJson.FromJSON(serializationData, Type.GetType(serializationType));
                }
                else
                {
                    plugin = (T)SimpleJson.FromJSON(serializationData, Type.GetType(serializationType + "," + serializationNamespace));
                }
            }
            catch
            {
                StringExtensions.LogError("ItemPluginWrapper", "Ctor", "Unable to deserialize: " + serializationNamespace + ", " + serializationType);
            }
        }

        #endregion

        #region Public Methods

        public UniversalPluginWrapper<T> Clone()
        {
            UniversalPluginWrapper<T> result = new UniversalPluginWrapper<T>();

            if (string.IsNullOrEmpty(serializationNamespace))
            {
                result.plugin = (T)SimpleJson.FromJSON(serializationData, Type.GetType(serializationType));
            }
            else
            {
                result.plugin = (T)SimpleJson.FromJSON(serializationData, Type.GetType(serializationType + "," + serializationNamespace));
            }

            result.serializationData = serializationData;
            result.serializationNamespace = serializationNamespace;
            result.serializationType = serializationType;

            return result;
        }

        public void DataLoad(Stream stream)
        {
            serializationType = stream.ReadStringPacket();
            serializationData = stream.ReadStringPacket();
            serializationNamespace = stream.ReadStringPacket();
            id = stream.ReadStringPacket();
        }

        public static jsonUniversalPluginWrapper JSONFromDataLoad(Stream stream)
        {
            jsonUniversalPluginWrapper result = new jsonUniversalPluginWrapper();

            result.serializationType = stream.ReadStringPacket();
            result.serializationData = stream.ReadStringPacket();
            result.serializationNamespace = stream.ReadStringPacket();
            result.id = stream.ReadStringPacket();

            return result;
        }

        public void DataSave(Stream stream)
        {
            stream.WriteStringPacket(serializationType);
            stream.WriteStringPacket(serializationData);
            stream.WriteStringPacket(serializationNamespace);
            stream.WriteStringPacket(id);
        }

        public jsonUniversalPluginWrapper ToJSON()
        {
            jsonUniversalPluginWrapper result = new jsonUniversalPluginWrapper();

            result.serializationData = serializationData;
            result.serializationNamespace = serializationNamespace;
            result.serializationType = serializationType;
            result.id = id;

            return result;
        }

        #endregion

        #region Serialization

        [JsonAfterDeserialization]
        public void OnAfterDeserialize()
        {
            try
            {
                if (string.IsNullOrEmpty(serializationData)) return;

                if (string.IsNullOrEmpty(serializationNamespace))
                {
                    plugin = (T)SimpleJson.FromJSON(serializationData, Type.GetType(serializationType));
                }
                else
                {
                    plugin = (T)SimpleJson.FromJSON(serializationData, Type.GetType(serializationType + "," + serializationNamespace));
                }
            }
            catch { }
        }

        public void OnBeforeSerialize() { }

        #endregion

    }
}